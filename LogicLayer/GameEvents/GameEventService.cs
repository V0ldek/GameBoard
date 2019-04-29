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

        public Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGameEventAsync(int gameEventId)
        {
            throw new NotImplementedException();
        }

        public Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userId)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventDto> GetGameEventAsync([NotNull] GameEventListItemDto gameEventListItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventPermission> GetGameEventPermissionByUserAsync(int gameEventId, [NotNull] string userId)
        {
            throw new NotImplementedException();
        }

        public Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserId)
        {
            throw new NotImplementedException();
        }

        public Task AcceptGameEventInvitationAsync([NotNull] int gameEventId, [NotNull] string invitedUserId)
        {
            throw new NotImplementedException();
        }

        public Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userId)
        {
            throw new NotImplementedException();
        }
    }
}
