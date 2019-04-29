using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListDto
    {
        [NotNull]
        [ItemNotNull]
        public List<GameEventListItemDto> AcceptedGameEvents { get; }

        [NotNull]
        [ItemNotNull]
        public List<GameEventListItemDto> PendingGameEvents { get; }

        [NotNull]
        [ItemNotNull]
        public List<GameEventListItemDto> CreatorGameEvents { get; }

        public GameEventListDto(
            [NotNull] [ItemNotNull] List<GameEventListItemDto> acceptedGameEvents,
            [NotNull] [ItemNotNull] List<GameEventListItemDto> pendingGameEvents,
            [NotNull] [ItemNotNull] List<GameEventListItemDto> creatorGameEvents)
        {
            AcceptedGameEvents = acceptedGameEvents;
            PendingGameEvents = pendingGameEvents;
            CreatorGameEvents = creatorGameEvents;
        }
    }
}
