using GameBoard.DataLayer.Enums;

namespace GameBoard.DataLayer.Entities
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GameEventId { get; set; }
        public GameEvent GameEvent { get; set; }

        public GameStatus GameStatus { get; set; }
    }
}
