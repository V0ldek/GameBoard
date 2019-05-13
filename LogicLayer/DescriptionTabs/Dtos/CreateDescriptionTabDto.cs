using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public class CreateDescriptionTabDto
    {
        public int GameEventId { get; }

        [NotNull]
        public string Description { get; }

        public CreateDescriptionTabDto(int gameEventId, [NotNull] string description)
        {
            GameEventId = gameEventId;
            Description = description;
        }
    }
}
