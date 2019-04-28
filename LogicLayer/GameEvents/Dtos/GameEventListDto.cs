using System.Collections.Generic;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListDto
    {
        List<GameEventListItemDto> AcceptedGameEvents { get; set; }
        List<GameEventListItemDto> PendingGameEvents { get; set; }
    }
}
