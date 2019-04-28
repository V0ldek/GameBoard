using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task CreateGameEventAsync(CreateGameEventDto requestedGameEvent, IEnumerable<string> games) => throw new NotImplementedException();
        public async Task DeleteGameEventAsync(string gameEventId) => throw new NotImplementedException();

        public async Task EditGameEventAsync(EditGameEventDto editedEvent, IEnumerable<string> newGames) => throw new NotImplementedException();
        public async Task<IEnumerable<GameEventDto>> GetAccessibleGameEventsAsync(string userId) => throw new NotImplementedException();

        public async Task<GameEventPermission> GetGameEventParticipationByUserAsync(string gameEventId, string userId) => throw new NotImplementedException();

        public async Task<GameEventDto> GetGameEventAsync(string gameEventId) => throw new NotImplementedException();

        public async Task SendGameEventInvitationAsync(string gameEventId, string userId) => throw new NotImplementedException();

        public async Task AcceptGameEventInvitationAsync(string gameEventId) => throw new NotImplementedException();

        public async Task RejectGameEventInvitationAsync(string gameEventId) => throw new NotImplementedException();
    }
}
