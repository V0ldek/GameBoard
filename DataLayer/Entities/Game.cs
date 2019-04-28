using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.DataLayer.Entities
{
    public class Game
    {
        public string Name { get; set; }

        public int GameEventId { get; set; }
        public GameEvent GameEvent { get; set; }

    }
}
