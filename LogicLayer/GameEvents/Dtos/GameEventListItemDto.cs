using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListItemDto
    {
        public int GameEventId { get; }

        [NotNull]
        public string GameEventName { get; }

        [NotNull]
        public string CreatorName { get; }

        public GameEventListItemDto(
            int gameEventId,
            [NotNull] string gameEventName,
            [NotNull] string creatorName)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            CreatorName = creatorName;
        }
    }
}
