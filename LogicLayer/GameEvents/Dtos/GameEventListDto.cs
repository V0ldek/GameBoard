using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListDto
    {
        [NotNull]
        [ItemNotNull]
        List<GameEventListItemDto> AcceptedGameEvents { get; }

        [NotNull]
        [ItemNotNull]
        List<GameEventListItemDto> PendingGameEvents { get; }

        [NotNull]
        [ItemNotNull]
        List<GameEventListItemDto> CreatorGameEvents { get; }

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
