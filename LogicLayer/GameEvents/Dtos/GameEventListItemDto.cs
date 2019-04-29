using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListItemDto
    {
        public int GameEventId { get; }

        [NotNull]
        public string GameEventName { get; }

        [NotNull]
        public UserDto Creator { get; }

        public GameEventListItemDto(
            int gameEventId,
            [NotNull] string gameEventName,
            [NotNull] UserDto creator)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            Creator = creator;
        }
    }
}
