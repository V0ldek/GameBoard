using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public class DescriptionTabDto
    {
        [NotNull]
        public string Description { get; }

        public DescriptionTabDto([NotNull] string description)
        {
            Description = description;
        }
    }
}