using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;

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

        public async Task DeleteGameEventAsync(string gameEventId) => throw new NotImplementedException();

        public async Task EditGameEventAsync(EditGameEventDto editedEvent, IEnumerable<string> newGames) => throw new NotImplementedException();

        public async Task<GameEventDto> GetGameEventAsync(GameEventListItemDto gameEventListItemDto) => throw new NotImplementedException();

        public Task<IEnumerable<GameEventListDto>> GetAccessibleGameEventsAsync([NotNull] string userId) => throw new NotImplementedException();

        public Task<GameEventPermission> GetGameEventPermissionByUserAsync([NotNull] string gameEventId, [NotNull] string userId) => throw new NotImplementedException();

        public async Task SendGameEventInvitationAsync(string gameEventId, string userId) => throw new NotImplementedException();

        public async Task AcceptGameEventInvitationAsync(string gameEventId) => throw new NotImplementedException();

        public async Task RejectGameEventInvitationAsync(string gameEventId) => throw new NotImplementedException();

    }
}
