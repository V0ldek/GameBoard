using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents
{
    // Perhaps this interface should be split into three separate interfaces.
    public interface IGameEventService
    {
        // Throws CreateGameEventException when new GameEvent could not be created (what can go wrong?)
        Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent);

        // Throws DeleteGameEventException when GameEvent could not be deleted (Event with such id doesn't exist, but I don't know if that is actually possible)
        Task DeleteGameEventAsync(int gameEventId);

        Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent);

        // Returns Task with null if game event does not exist.
        [ItemCanBeNull]
        Task<GameEventDto> GetGameEventAsync([NotNull] GameEventListItemDto gameEventListItemDto);

        Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userId);

        Task<GameEventPermission> GetGameEventPermissionByUserAsync(int gameEventId, [NotNull] string userId);

        Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userId);

        Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserId);

        Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserId);
    }
}
