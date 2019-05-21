namespace GameBoard.DataLayer.Entities
{
    public class DescriptionTab
    {
        public int Id { get; set; }

        public int GameEventId { get; set; }

        public GameEvent GameEvent { get; set; }

        public string Description { get; set; }
    }
}
