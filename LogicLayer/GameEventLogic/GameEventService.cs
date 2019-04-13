using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventLogic.Dtos;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.GameEventLogic
{
    class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> CreateGameEvent([NotNull] CreateGameEventDto requestedGameEvent)
        {
            // There is no checking if UserId is valid;

            _repository.GameEvents.Add(new GameEvent()
            {
                CreatorId = requestedGameEvent.UserId,

                MeetingTime = DateTime.ParseExact(requestedGameEvent.MeetingTime, "d", null),

                Place = requestedGameEvent.Place
                
            });

            throw new NotImplementedException();
        }

        public Task<IEnumerable<GameEventDto>> FindGameEventsByUserId([NotNull] string userId)
        {
            throw new NotImplementedException();
        }
    }
}
