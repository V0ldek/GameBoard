using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using GameBoard.LogicLayer.GameEventLogic.Dtos;

namespace GameBoard.LogicLayer.GameEventLogic
{
    public interface IGameEventService
    {
        [NotNull]
        Task<IEnumerable<GameEventDto>> FindGameEventsByUserId([NotNull] string userId);

        [NotNull]
        Task<bool> CreateGameEvent([NotNull] CreateGameEventDto requestedGameEvent); 

    }
}
