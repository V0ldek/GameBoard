using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.GameEventInvitations
{
    internal class GameEventInvitationsService : IGameEventInvitationsService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventInvitationsService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        private IQueryable<GameEventParticipation> GetGameEventParticipationsInOneEvent(
            int gameEventId,
            [NotNull] string userName)
        {
            return _repository
                .GetUserByUserName(userName)
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

        public async Task SendGameEventInvitationAsync(int gameEventId, string userName)
        {
            var participation = await
                GetGameEventParticipationsInOneEvent(gameEventId, userName)
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

            string userId = await _repository.GetUserIdByUserName(userName);

            var gameEventParticipation = new GameEventParticipation
            {
                TakesPartInId = gameEventId,
                ParticipantId = userId,
                ParticipationStatus = ParticipationStatus.PendingGuest
            };
            _repository.GameEventParticipations.Add(gameEventParticipation);

            await _repository.SaveChangesAsync();

            //TODO: send email with an invitation
        }
    }
}
