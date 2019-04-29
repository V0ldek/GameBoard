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

        public GameEventListDto(
            [NotNull] [ItemNotNull] List<GameEventListItemDto> acceptedGameEvents,
            [NotNull] [ItemNotNull] List<GameEventListItemDto> pendingGameEvents)
        {
            AcceptedGameEvents = acceptedGameEvents;
            PendingGameEvents = pendingGameEvents;
        }
    }
}
