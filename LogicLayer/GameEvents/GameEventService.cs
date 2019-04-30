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
using System.Data.SqlClient;


namespace GameBoard.LogicLayer.GameEvents
{
    class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent)
        {
            var creatorParticipation = new GameEventParticipation() { ParticipantId = requestedGameEvent.CreatorUserName };
            var gameEvent = new GameEvent()
            {
                EventName = requestedGameEvent.GameEventName,
                MeetingTime = requestedGameEvent.MeetingTime,
                Place = requestedGameEvent.Place,
                Games = requestedGameEvent.Games.Select(g => new Game { Name = g }).ToList(),
                Participations = new List<GameEventParticipation>()
            };
            gameEvent.Participations.Add(creatorParticipation);

            return _repository.SaveChangesAsync();
        }

        public async Task DeleteGameEventAsync(int gameEventId)
        {
            var gameEvent = await _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Participations)
                .Include(ge => ge.Games)
                .SingleAsync();

            foreach (var game in gameEvent.Games)
            {
                _repository.Games.Remove(game);
            }
            foreach (var participation in gameEvent.Participations)
            {
                _repository.GameEventParticipations.Remove(participation);
            }
            _repository.GameEvents.Remove(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent)
        {
            var gameEvent = await _repository.GameEvents
                .Include(ge => ge.Games)
                .SingleAsync(ge => ge.Id == editedEvent.GameEventId);

            gameEvent.EventName = editedEvent.GameEventName ?? gameEvent.EventName;
            gameEvent.MeetingTime = editedEvent.MeetingTime ?? gameEvent.MeetingTime;
            gameEvent.Place = editedEvent.Place ?? gameEvent.Place;

            foreach (var game in gameEvent.Games)
            {
                _repository.Games.Remove(game); //not sure if it is correct to remove data.
            }
            gameEvent.Games = editedEvent.Games.Select(g => new Game { Name = g }).ToList();

            await _repository.SaveChangesAsync();
        }

        private Task<List<GameEventListItemDto>> GetGameEventsWithSamePartitipationStatus([NotNull] string userName, ParticipationStatus participationStatus)
        {
            var normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserName);

            var gameEvents = user
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

        public async Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userName)
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
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Paticipant)
                .SingleAsync();

            return gameEvent.ToGameEventDto();
        }

        public Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            throw new NotImplementedException();
        }

        public Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            throw new NotImplementedException();
        }

        public Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userName)
        {
            throw new NotImplementedException();
        }
    }
}
