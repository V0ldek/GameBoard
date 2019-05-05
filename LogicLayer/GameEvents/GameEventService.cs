using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.GameEvents
{
    internal class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateGameEventAsync(CreateGameEventDto requestedGameEvent)
        {
            var creatorId = await _repository.GetUserIdByUserName(requestedGameEvent.CreatorUserName);
            var creatorParticipation = new GameEventParticipation
            {
                ParticipantId = creatorId,
                ParticipationStatus = ParticipationStatus.Creator
            };
            var gameEvent = new GameEvent
            {
                Name = requestedGameEvent.Name,
                MeetingTime = requestedGameEvent.MeetingTime,
                Place = requestedGameEvent.Place,
                Games = requestedGameEvent.Games.Select(g => new Game { Name = g }).ToList(),
                Participations = new List<GameEventParticipation>()
            };

            gameEvent.Participations.Add(creatorParticipation); // I don't know why, but it adds a participation with pendingInvitation status.
            _repository.GameEvents.Add(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync(EditGameEventDto editedEvent)
        {
            var gameEvent = await _repository.GameEvents
                .Include(ge => ge.Games)
                .SingleAsync(ge => ge.Id == editedEvent.Id);

            gameEvent.Name = editedEvent.Name; // I think we should always overwrite the previous data.
            gameEvent.MeetingTime = editedEvent.MeetingTime; // Here too.
            gameEvent.Place = editedEvent.Place; // Here too.

            foreach (var game in gameEvent.Games)
            {
                _repository.Games.Remove(game); //not sure if it is correct to remove data. What is the other option then?
            }
            gameEvent.Games = editedEvent.Games.Select(g => new Game { Name = g }).ToList();

            await _repository.SaveChangesAsync();
        }

        private Task<List<GameEventListItemDto>> GetGameEventsWithSamePartitipationStatus(string userName, ParticipationStatus participationStatus)
        {
            var gameEvents = _repository
                .GetUserByUserName(userName)
                .Include(u => u.Participations)
                .SelectMany(u => u.Participations)
                .Where(p => p.ParticipationStatus == participationStatus)
                .Include(p => p.TakesPartIn)
                .Select(p => p.TakesPartIn);

            return gameEvents
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Paticipant)
                .Select(ge => ge.ToGameEventListItemDto()) // the conversion should be outside the query, but then the query won't be optimized. Well... I don't know if it will be optimized with what we have right now. We need also a filtered index on Creator.
                .ToListAsync();
        }

        public async Task<GameEventListDto> GetAccessibleGameEventsAsync(string userName)
        {
            var creatorGameEvents =
                GetGameEventsWithSamePartitipationStatus(userName, ParticipationStatus.Creator);
            var invitees =
                GetGameEventsWithSamePartitipationStatus(userName, ParticipationStatus.PendingGuest);
            var participants =
                GetGameEventsWithSamePartitipationStatus(userName, ParticipationStatus.AcceptedGuest);
            // Perhaps it should be only one query with the succeding conversion.

            return new GameEventListDto(
                await creatorGameEvents,
                await invitees,
                await participants);
        }

        public async Task<GameEventDto> GetGameEventAsync(int gameEventId)
        {
            var gameEvent = await _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Games)
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Paticipant)
                .SingleAsync();

            return gameEvent.ToGameEventDto();
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
                        throw new ArgumentOutOfRangeException(); //maybe some message?

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
        }
    }
}
