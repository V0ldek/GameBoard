using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListDto
    {
        List<GameEventDto> AcceptedGameEvents { get; set; }
        List<GameEventDto> PendingGameEvents { get; set; }
    }
}
