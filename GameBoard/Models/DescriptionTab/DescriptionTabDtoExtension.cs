using GameBoard.LogicLayer.DescriptionTabs.Dtos;

namespace GameBoard.Models.DescriptionTab
{
    public static class DescriptionTabDtoExtension
    {
        public static DescriptionTabViewModel ToViewModel(this DescriptionTabDto descriptionTab) =>
            new DescriptionTabViewModel
            {
                Description = descriptionTab.Description
            };
    }
}