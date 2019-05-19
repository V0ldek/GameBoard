using GameBoard.LogicLayer.EventTabs.Dtos;

namespace GameBoard.Models.EventTab
{
    public static class EventTabDtoExtension
    {
        public static DescriptionTabViewModel ToViewModel(this DescriptionTabDto descriptionTab) =>
            new DescriptionTabViewModel
            {
                Description = descriptionTab.Description
            };
    }
}