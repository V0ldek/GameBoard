using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using GameBoard.LogicLayer.GameEvent.Dtos;

namespace GameBoard.LogicLayer.GameEvent
{
    public interface IGameEventService
    {
        [NotNull]
        Task<IEnumerable<GameEventDto>> FindGameEventsByUserId([NotNull] string userId);

        [NotNull]
        Task<bool> CreateGameEvent([NotNull] CreateGameEventDto requestedGameEvent); 
    }
}
