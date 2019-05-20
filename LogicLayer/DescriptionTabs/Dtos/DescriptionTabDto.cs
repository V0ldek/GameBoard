using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public class DescriptionTabDto
    {
        public int Id { get; }

        public int GameEventId { get; }

        [CanBeNull]
        public string Description { get; }

        public DescriptionTabDto(int id, int gameEventId ,[CanBeNull] string description)
        {
            Id = id;
            GameEventId = gameEventId;
            Description = description;
        }
    }
}