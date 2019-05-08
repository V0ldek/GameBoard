namespace GameBoard.LogicLayer.EventTabs.Dtos
{
    public class GameEventTabsDto
    {
        public DescriptionTabDto Description { get; }

        public GameEventTabsDto(DescriptionTabDto description)
        {
            Description = description;
        }
    }
}