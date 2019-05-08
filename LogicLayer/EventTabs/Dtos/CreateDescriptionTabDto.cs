using JetBrains.Annotations;

namespace GameBoard.LogicLayer.EventTabs.Dtos
{
    public class CreateDescriptionTabDto
    {
        public int DescriptionOfId { get; }
        public string Description { get; }

        public CreateDescriptionTabDto(int descriptionOfId, [NotNull] string description)
        {
            DescriptionOfId = descriptionOfId;
            Description = description;
        }
    }
}
