using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEventParticipations.Dtos;
using GameBoard.LogicLayer.GameEventParticipations.Exceptions;
using GameBoard.LogicLayer.Notifications;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.GameEventParticipations
{
    internal sealed class GameEventParticipationService : IGameEventParticipationService
    {
        private readonly IMailSender _mailSender;
        private readonly IGameBoardRepository _repository;

        public GameEventParticipationService(IGameBoardRepository repository, IMailSender mailSender)
        {
            _repository = repository;
            _mailSender = mailSender;
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
                    case ParticipationStatus.RejectedGuest:
                        throw new ArgumentOutOfRangeException(
                            nameof(ParticipationStatus.RejectedGuest),
                            "We queried for a non-rejected participation, so maybe look for an error there");
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(participation.ParticipationStatus),
                            $"The value {participation.ParticipationStatus} of ParticipationStatus " +
                            "is not included in this switch statement.");
                }
            }

            var userTo = await _repository.ApplicationUsers.SingleAsync(ApplicationUser.UserNameEquals(userNameTo));

            await CreateNewGameEventParticipation(gameEventId, userTo);

            await SendGameEventInvitationAsync(gameEventId, userTo, gameEventInvitationDto.GenerateGameEventLink);
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
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(userParticipation.ParticipationStatus),
                        $"Unexpected value in ${nameof(ExitGameEventAsync)} - ${userParticipation.ParticipationStatus}");
            }

            await ChangeGameEventParticipationStatusAsync(userParticipation, ParticipationStatus.ExitedGuest);
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
                        p.ParticipationStatus != ParticipationStatus.ExitedGuest);

        private IQueryable<GameEventParticipation> GetGameEventParticipationsInOneEvent(
            int gameEventId,
            [NotNull] string userName) =>
            _repository.ApplicationUsers
                .Where(ApplicationUser.UserNameEquals(userName))
                .Include(u => u.Participations)
                .SelectMany(u => u.Participations)
                .Where(p => p.TakesPartInId == gameEventId);

        private async Task CreateNewGameEventParticipation(int gameEventId, [NotNull] ApplicationUser userTo)
        {
            var gameEventParticipation = new GameEventParticipation
            {
                TakesPartInId = gameEventId,
                ParticipantId = userTo.Id,
                ParticipationStatus = ParticipationStatus.PendingGuest
            };
            _repository.GameEventParticipations.Add(gameEventParticipation);

            await _repository.SaveChangesAsync();
        }

        private Task SendGameEventInvitationAsync(
            int gameEventId,
            [NotNull] ApplicationUser userTo,
            SendGameEventInvitationDto.GameEventLinkGenerator gameEventLinkGenerator) =>
            _mailSender.SendEventInvitationAsync(
                new List<string> {userTo.Email},
                gameEventLinkGenerator(gameEventId.ToString()));
    }
}