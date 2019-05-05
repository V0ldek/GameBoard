using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents
{
    public interface IGameEventService
    {
        Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent);

        Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent);

        // Returns Task with null if game event does not exist.
        [ItemCanBeNull]
        Task<GameEventDto> GetGameEventAsync(int gameEventId);

        [NotNull]
        [ItemNotNull]
        Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userName);
    }
}
