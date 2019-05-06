using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEventInvites.Dtos;
using GameBoard.LogicLayer.Notifications;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.GameEventInvites
{
    internal class GameEventInvitesService : IGameEventInvitesService
    {
        private readonly IMailSender _mailSender;
        private readonly IGameBoardRepository _repository;

        public GameEventInvitesService(IGameBoardRepository repository, IMailSender mailSender)
        {
            _repository = repository;
            _mailSender = mailSender;
        }

        public Task RejectGameEventInvitationAsync(int gameEventId, string invitedUserName) =>
            ChangeGameEventInvitationStatusAsync(
                gameEventId,
                invitedUserName,
                ParticipationStatus.RejectedGuest);

        public Task AcceptGameEventInvitationAsync(int gameEventId, string invitedUserName) =>
            ChangeGameEventInvitationStatusAsync(
                gameEventId,
                invitedUserName,
                ParticipationStatus.AcceptedGuest);

        private async Task ChangeGameEventInvitationStatusAsync(
            int gameEventId,
            [NotNull] string invitedUserName,
            ParticipationStatus participationStatus)
        {
            var participation = await
                GetGameEventParticipationsInOneEvent(gameEventId, invitedUserName)
                    .SingleAsync(p => p.ParticipationStatus == ParticipationStatus.PendingGuest);

            participation.ParticipationStatus = participationStatus;

            await _repository.SaveChangesAsync();
        }

        public async Task SendGameEventInvitationAsync(CreateGameEventInvitationDto gameEventInvitationDto)
        {
            var gameEventId = gameEventInvitationDto.GameEventId;
            var userNameTo = gameEventInvitationDto.UserNameTo;

            var participation = await GetNotRejectedGameEventParticipation(gameEventId, userNameTo);

            if (participation != null)
            {
                switch (participation.ParticipationStatus)
                {
                    case ParticipationStatus.PendingGuest:
                        throw new ApplicationException("You have already invited this user to this event.");
                    case ParticipationStatus.AcceptedGuest:
                        throw new ApplicationException("This user already participates in this event.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var userTo = await _repository.ApplicationUsers.SingleAsync(ApplicationUser.UserNameEquals(userNameTo));

            await CreateNewGameEventParticipation(gameEventId, userTo);

            await SendGameEventInvitationAsync(gameEventId, userTo, gameEventInvitationDto.GenerateGameEventLink);
        }

        private Task<GameEventParticipation> GetNotRejectedGameEventParticipation(
            int gameEventId,
            [NotNull] string userName) =>
            GetGameEventParticipationsInOneEvent(gameEventId, userName)
                .SingleOrDefaultAsync(p => p.ParticipationStatus != ParticipationStatus.RejectedGuest);

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
            CreateGameEventInvitationDto.GameEventLinkGenerator gameEventLinkGenerator) =>
            _mailSender.SendEventInvitationAsync(
                new List<string> {userTo.Email},
                gameEventLinkGenerator(gameEventId.ToString()));
    }
}