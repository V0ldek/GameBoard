using System.Collections.Generic;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListDto
    {
        List<GameEventDto> AcceptedGameEvents { get; set; }
        List<GameEventDto> PendingGameEvents { get; set; }
    }
}
