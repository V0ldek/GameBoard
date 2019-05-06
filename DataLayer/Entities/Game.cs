namespace GameBoard.DataLayer.Entities
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GameEventId { get; set; }
        public GameEvent GameEvent { get; set; }

        public int? PositionOnTheList { get; set; } // equals null if the game has been removed from the list.
    }
}