using JetBrains.Annotations;

namespace GameBoard.LogicLayer.EventTabs.Dtos
{
    public class DescriptionTabDto
    {
        [CanBeNull]
        public string Description { get; }

        public DescriptionTabDto([CanBeNull] string description)
        {
            Description = description;
        }
    }
}