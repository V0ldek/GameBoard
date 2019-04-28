using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents
{
    class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public Task AcceptGameEventInvitationAsync([NotNull] int gameEventId)
        {
            throw new NotImplementedException();
        }

        public Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent, [NotNull] IEnumerable<string> games)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGameEventAsync([NotNull] int gameEventId)
        {
            throw new NotImplementedException();
        }

        public Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent, [NotNull] IEnumerable<string> newGames)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GameEventListDto>> GetAccessibleGameEventsAsync([NotNull] string userId)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventDto> GetGameEventAsync([NotNull] GameEventListItemDto gameEventListItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventPermission> GetGameEventPermissionByUserAsync([NotNull] int gameEventId, [NotNull] string userId)
        {
            throw new NotImplementedException();
        }

        public Task RejectGameEventInvitationAsync([NotNull] int gameEventId)
        {
            throw new NotImplementedException();
        }

        public Task SendGameEventInvitationAsync([NotNull] int gameEventId, [NotNull] string userId)
        {
            throw new NotImplementedException();
        }
    }
}
