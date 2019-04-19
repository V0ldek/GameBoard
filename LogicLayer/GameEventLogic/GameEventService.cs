using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventLogic.Dtos;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.GameEventLogic
{
    class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateGameEvent(CreateGameEventDto requestedGameEvent)
        {
            _repository.GameEvents.Add(
                    new GameEvent
                    {
                        CreatorId = requestedGameEvent.UserId,
                        MeetingTime = DateTime.ParseExact(requestedGameEvent.MeetingTime, "yyyy-MM-dd HH:mm", null),
                        Place = requestedGameEvent.Place
                    });
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is SqlException sqlException)
            {
                switch (sqlException.Number)
                {
                    //case :
                }

            }
        }

        public async Task DeleteGameEvent(string gameEventId, string creatorId) => throw new NotImplementedException();

        public async Task EditGameEvent(CreateGameEventDto changedProperties, IEnumerable<string> newGames, IEnumerable<string> deleteGames) => throw new NotImplementedException();

        public async Task<IEnumerable<GameEventDto>> GetGameEventsByCreatorId(string creatorId) => throw new NotImplementedException();

        public async Task<IEnumerable<GameEventDto>> GetAcceptedGameEvents(string userId) => throw new NotImplementedException();
    }
}
