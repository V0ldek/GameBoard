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
            var creatorParticipation = new GameEventParticipation() { ParticipantId = requestedGameEvent.CreatorId };
            var gameEvent = new GameEvent()
            {
                EventName = requestedGameEvent.GameEventName,
                MeetingTime = requestedGameEvent.MeetingTime,
                Place = requestedGameEvent.Place,
                Games = games.Select(g => new Game { Name = g }).ToList(),
                Participations = new List<GameEventParticipation>()
            };
            gameEvent.Participations.Add(creatorParticipation);

            await _repository.SaveChangesAsync();
        }

        public Task DeleteGameEventAsync(int gameEventId)
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

        public Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent)
        {
            var gameEvent = await _repository.GameEvents
                .Include(ge => ge.Games)
                .SingleAsync(ge => ge.Id == editedEvent.GameEventId);

            if (editedEvent.GameEventName != null && editedEvent.GameEventName != "") gameEvent.EventName = editedEvent.GameEventName;
            if (editedEvent.IsMeetingTimeSet()) gameEvent.MeetingTime = editedEvent.MeetingTime;
            if (editedEvent.Place != null) gameEvent.Place = editedEvent.Place;

            foreach (var game in gameEvent.Games)
            {
                _repository.Games.Remove(game);
            }
            gameEvent.Games = newGames.Select(g => new Game { Name = g }).ToList();

            await _repository.SaveChangesAsync();
        }


        public Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userName)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventDto> GetGameEventAsync(int gameEventId)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventPermission> GetGameEventPermissionByUserAsync(int gameEventId, [NotNull] string userName)
        {
            throw new NotImplementedException();
        }

        public Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            throw new NotImplementedException();
        }

        public Task AcceptGameEventInvitationAsync([NotNull] int gameEventId, [NotNull] string invitedUserName)
        {
            throw new NotImplementedException();
        }

        public Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userName)
        {
            throw new NotImplementedException();
        }
    }
}
