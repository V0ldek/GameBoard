using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventLogic.Dtos;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using GameBoard.DataLayer.Enums;

namespace GameBoard.LogicLayer.GameEventLogic
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
            var gameEvent = new GameEvent()
            {
                CreatorId = requestedGameEvent.CreatorId,
                MeetingTime = requestedGameEvent.MeetingTime,
                Place = requestedGameEvent.Place,
                Games = games.Select(g => new Game { Name = g }).ToList()
            };
            _repository.GameEvents.Add(gameEvent);

            await _repository.SaveChangesAsync();

        }

        public async Task DeleteGameEventAsync(string gameEventId)
        {
                var gameEvent = _repository.GameEvents
                    .Include(ge => ge.Games)
                    .Include(ge => ge.Invitations)
                    .Single(ge => gameEventId == ge.Id);

                foreach (var game in gameEvent.Games)
                {
                    _repository.Games.Remove(game);
                }

                foreach (var invite in gameEvent.Invitations)
                {
                    _repository.GameEventInvitations.Remove(invite);
                }

                _repository.GameEvents.Remove(gameEvent);

                await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync(EditGameEventDto editedEvent, IEnumerable<string> newGames)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameEventDto>> GetAccessibleGameEventsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<GameEventPermission> GetGameEventParticipationByUserAsync(string gameEventId, string userId) => throw new NotImplementedException();

        public async Task<GameEventDto> GetGameEventAsync(string gameEventId) => throw new NotImplementedException();

        public async Task SendGameEventInvitationAsync(string gameEventId, string userId) => throw new NotImplementedException();

        public async Task AcceptGameEventInvitationAsync(string gameEventId) => throw new NotImplementedException();

        public async Task RejectGameEventInvitationAsync(string gameEventId) => throw new NotImplementedException();
    }
}
