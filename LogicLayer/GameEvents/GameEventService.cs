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

        public async Task CreateGameEventAsync(CreateGameEventDto requestedGameEvent, IEnumerable<string> games)
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

        public async Task EditGameEventAsync(EditGameEventDto editedEvent, IEnumerable<string> newGames) => throw new NotImplementedException();

        public async Task<GameEventDto> GetGameEventAsync(GameEventListItemDto gameEventListItemDto) => throw new NotImplementedException();

        public Task<IEnumerable<GameEventListDto>> GetAccessibleGameEventsAsync([NotNull] string userId) => throw new NotImplementedException();

        public Task<GameEventPermission> GetGameEventPermissionByUserAsync([NotNull] int gameEventId, [NotNull] string userId) => throw new NotImplementedException();

        public async Task SendGameEventInvitationAsync(int gameEventId, string userId) => throw new NotImplementedException();

        public async Task AcceptGameEventInvitationAsync(int gameEventId) => throw new NotImplementedException();

        public async Task RejectGameEventInvitationAsync(int gameEventId) => throw new NotImplementedException();

    }
}
