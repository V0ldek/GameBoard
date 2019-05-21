using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEventParticipations.Dtos;
using GameBoard.LogicLayer.GameEventParticipations.Exceptions;
using GameBoard.LogicLayer.GameEventParticipations.Notifications;
using GameBoard.LogicLayer.Notifications;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.GameEventParticipations
{
    internal sealed class GameEventParticipationService : IGameEventParticipationService
    {
        private readonly INotificationService _notificationService;
        private readonly IGameBoardRepository _repository;

        public GameEventParticipationService(IGameBoardRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task SendGameEventInvitationAsync(SendGameEventInvitationDto gameEventInvitationDto)
        {
            var gameEventId = gameEventInvitationDto.GameEventId;
            var userNameTo = gameEventInvitationDto.UserNameTo;

            var participation = await GetActiveGameEventParticipation(gameEventId, userNameTo);

            if (participation != null)
            {
                switch (participation.ParticipationStatus)
                {
                    case ParticipationStatus.PendingGuest:
                        throw new GameEventParticipationException("You have already invited this user to this event.");
                    case ParticipationStatus.AcceptedGuest:
                        throw new GameEventParticipationException("This user already participates in this event.");
                    case ParticipationStatus.Creator:
                        throw new GameEventParticipationException("You cannot invite yourself.");
                    // Nothing else can be returned by GetActiveGameEventParticipation.
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(participation.ParticipationStatus),
                            $"The value {participation.ParticipationStatus} of ParticipationStatus " +
                            "is not included in this switch statement.");
                }
            }

            var userTo = await _repository.ApplicationUsers.SingleAsync(ApplicationUser.UserNameEquals(userNameTo));

            using (var transaction = _repository.BeginTransaction())
            {
                await CreateNewGameEventParticipation(gameEventId, userTo.Id);
                await SendGameEventInvitationAsync(
                    gameEventInvitationDto.GameEventId,
                    userTo,
                    gameEventInvitationDto.GenerateGameEventLink);
                transaction.Commit();
            }
        }

        public async Task SendGameEventInvitationAsync(IEnumerable<SendGameEventInvitationDto> gameEventInvitationDtos)
        {
            using (var transaction = _repository.BeginTransaction())
            {
                foreach (var gameEventInvitationDto in gameEventInvitationDtos)
                {
                    try
                    {
                        await SendGameEventInvitationAsync(gameEventInvitationDto);
                    }
                    catch (GameEventParticipationException)
                    {
                        // There is no need in throwing this exception,
                        // it means that some of the members of the group cannot be invited, so we invite all we can.
                    }
                }

                transaction.Commit();
            }
        }

        public async Task AcceptGameEventInvitationAsync(int gameEventId, string invitedUserName)
        {
            var userParticipation = await GetActiveGameEventParticipation(gameEventId, invitedUserName);
            await ChangeGameEventParticipationStatusAsync(
                userParticipation,
                ParticipationStatus.AcceptedGuest);
        }

        public async Task RejectGameEventInvitationAsync(int gameEventId, string invitedUserName)
        {
            var userParticipation = await GetActiveGameEventParticipation(gameEventId, invitedUserName);
            await ChangeGameEventParticipationStatusAsync(
                userParticipation,
                ParticipationStatus.RejectedGuest);
        }

        public async Task ExitGameEventAsync(int gameEventId, string userName)
        {
            var userParticipation = await GetActiveGameEventParticipation(gameEventId, userName);

            switch (userParticipation?.ParticipationStatus)
            {
                case ParticipationStatus.Creator:
                    throw new GameEventParticipationException("As a creator, you cannot exit your own event.");
                case ParticipationStatus.PendingGuest:
                case null:
                    throw new GameEventParticipationException("You cannot exit an event you haven't entered.");
                case ParticipationStatus.AcceptedGuest:
                    break;
                // Nothing else should be returned by GetActiveGameEventParticipation.
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(userParticipation.ParticipationStatus),
                        $"Unexpected value in ${nameof(ExitGameEventAsync)} - ${userParticipation.ParticipationStatus}");
            }

            await ChangeGameEventParticipationStatusAsync(
                userParticipation,
                ParticipationStatus.ExitedGuest);
        }

        public async Task RemoveFromGameEventAsync(int gameEventId, string userName)
        {
            var userParticipation = await GetActiveGameEventParticipation(gameEventId, userName);

            switch (userParticipation?.ParticipationStatus)
            {
                case ParticipationStatus.Creator:
                    throw new GameEventParticipationException("You cannot remove the creator.");
                case null:
                    throw new GameEventParticipationException("You cannot remove a user that has not been invited.");
                case ParticipationStatus.PendingGuest:
                case ParticipationStatus.AcceptedGuest:
                    break;
                // Nothing else should be returned by GetActiveGameEventParticipation.
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(userParticipation.ParticipationStatus),
                        $"Unexpected value in ${nameof(ExitGameEventAsync)} - ${userParticipation.ParticipationStatus}");
            }

            await ChangeGameEventParticipationStatusAsync(userParticipation, ParticipationStatus.RemovedGuest);
        }

        private async Task ChangeGameEventParticipationStatusAsync(
            GameEventParticipation participation,
            ParticipationStatus participationStatus)
        {
            participation.ParticipationStatus = participationStatus;

            await _repository.SaveChangesAsync();
        }

        private Task<GameEventParticipation> GetActiveGameEventParticipation(
            int gameEventId,
            [NotNull] string userName) =>
            GetGameEventParticipationsInOneEvent(gameEventId, userName)
                .SingleOrDefaultAsync(
                    p => p.ParticipationStatus != ParticipationStatus.RejectedGuest &&
                        p.ParticipationStatus != ParticipationStatus.ExitedGuest &&
                        p.ParticipationStatus != ParticipationStatus.RemovedGuest);

        private IQueryable<GameEventParticipation> GetGameEventParticipationsInOneEvent(
            int gameEventId,
            [NotNull] string userName) =>
            _repository.ApplicationUsers
                .Where(ApplicationUser.UserNameEquals(userName))
                .Include(u => u.Participations)
                .SelectMany(u => u.Participations)
                .Where(p => p.TakesPartInId == gameEventId);

        private async Task CreateNewGameEventParticipation(int gameEventId, string userToId)
        {
            var gameEventParticipation = new GameEventParticipation
            {
                TakesPartInId = gameEventId,
                ParticipantId = userToId,
                ParticipationStatus = ParticipationStatus.PendingGuest
            };
            _repository.GameEventParticipations.Add(gameEventParticipation);

            await _repository.SaveChangesAsync();
        }

        private async Task SendGameEventInvitationAsync(
            int gameEventId,
            [NotNull] ApplicationUser userTo,
            SendGameEventInvitationDto.GameEventLinkGenerator gameEventLinkGenerator)
        {
            var gameEventData = await _repository.GameEvents
                .Where(g => g.Id == gameEventId)
                .Include(g => g.Participations)
                .ThenInclude(p => p.Participant)
                .Select(
                    g => new
                    {
                        g.Id,
                        g.Name,
                        CreatorParticipations = g.Participations
                            .Where(p => p.ParticipationStatus == ParticipationStatus.Creator)
                    }).SingleAsync();

            var creatorName = gameEventData.CreatorParticipations.Single().Participant.UserName;

            var notification = new GameEventInvitationNotification(
                gameEventData.Name,
                creatorName,
                userTo.UserName,
                userTo.Email,
                gameEventLinkGenerator(gameEventId.ToString()));

            await _notificationService.CreateNotificationBatch(notification).SendAsync();
        }
    }
}