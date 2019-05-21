using JetBrains.Annotations;

namespace GameBoard.Models.DescriptionTab
{
    public class DescriptionTabViewModel
    {
        public int Id { get; set; }

        public int GameEventId { get; set; }

        [CanBeNull]
        public string Description { get; set; }
    }
}