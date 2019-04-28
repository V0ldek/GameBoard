using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListItemDto
    {
        public int GameEventId { get; set; }
        public string GameEventName { get; set; }
        public string CreatorName { get; set; }
    }
}
