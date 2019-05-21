using GameBoard.LogicLayer.DescriptionTabs.Dtos;

namespace GameBoard.Models.DescriptionTab
{
    public static class DescriptionTabDtoExtension
    {
        public static DescriptionTabViewModel ToViewModel(this DescriptionTabDto descriptionTab) =>
            new DescriptionTabViewModel
            {
                Id = descriptionTab.Id,
                Description = descriptionTab.Description
            };

        public static EditDescriptionTabViewModel ToEditViewModel(this DescriptionTabDto descriptionTab) =>
            new EditDescriptionTabViewModel
            {
                Id = descriptionTab.Id,
                GameEventId = descriptionTab.GameEventId,
                Description = descriptionTab.Description
            };
    }
}