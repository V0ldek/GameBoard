using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using GameBoard.LogicLayer.GameEventLogic.Dtos;
using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.GameEventLogic
{
    public interface IGameEventService
    {
        //Throws CreateGameEventException when new GameEvent could not be created
        Task CreateGameEvent([NotNull] CreateGameEventDto requestedGameEvent);
        
        //Throws DeleteGameEventException when GameEvent could not be deleted
        Task DeleteGameEvent([NotNull] string gameEventId, [NotNull] string creatorId);
       
        Task EditGameEvent([NotNull] CreateGameEventDto changedProperties, IEnumerable<string> newGames, IEnumerable<string> deleteGames);

        Task<IEnumerable<GameEventDto>> GetGameEventsByCreatorId([NotNull] string creatorId);

        Task<IEnumerable<GameEventDto>> GetAcceptedGameEvents([NotNull] string userId);

    }
}
