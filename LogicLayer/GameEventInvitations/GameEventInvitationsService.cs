using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventInvitations.Dtos;
using GameBoard.LogicLayer.Notifications;

namespace GameBoard.LogicLayer.GameEventInvitations
{
    internal class GameEventInvitationsService : IGameEventInvitationsService
    {
        private readonly IGameBoardRepository _repository;
        private readonly IMailSender _mailSender;

        public GameEventInvitationsService(IGameBoardRepository repository, IMailSender mailSender)
        {
            _repository = repository;
            _mailSender = mailSender;
        }

        private IQueryable<GameEventParticipation> GetGameEventParticipationsInOneEvent(
            int gameEventId,
            [NotNull] string userName)
        {
            return _repository.ApplicationUsers
                .Where(ApplicationUser.UserNameEquals(userName))
                .Include(u => u.Participations)
                .SelectMany(u => u.Participations)
                .Where(p => p.TakesPartInId == gameEventId);
        }

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

        public Task RejectGameEventInvitationAsync(int gameEventId, string invitedUserName)
        {
            return ChangeGameEventInvitationStatusAsync(
                gameEventId,
                invitedUserName,
                ParticipationStatus.RejectedGuest);
        }

        public Task AcceptGameEventInvitationAsync(int gameEventId, string invitedUserName)
        {
            return ChangeGameEventInvitationStatusAsync(
                gameEventId,
                invitedUserName,
                ParticipationStatus.AcceptedGuest);
        }

        public async Task SendGameEventInvitationAsync(CreateGameEventInvitationDto gameEventInvitationDto)
        {
            var gameEventId = gameEventInvitationDto.GameEventId;
            var userNameTo = gameEventInvitationDto.UserNameTo;

            var participation = await
                GetGameEventParticipationsInOneEvent(gameEventId, userNameTo)
                .SingleOrDefaultAsync(p => p.ParticipationStatus != ParticipationStatus.RejectedGuest);

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

            var user = await _repository.ApplicationUsers.SingleAsync(ApplicationUser.UserNameEquals(userNameTo));

            var gameEventParticipation = new GameEventParticipation
            {
                TakesPartInId = gameEventId,
                ParticipantId = user.Id,
                ParticipationStatus = ParticipationStatus.PendingGuest
            };
            _repository.GameEventParticipations.Add(gameEventParticipation);

            await _repository.SaveChangesAsync();

            await _mailSender.SendEventInvitationAsync(
                new List<string>{user.Email},
                gameEventInvitationDto.GenerateGameEventLink(gameEventId.ToString()));
        }
    }
}
