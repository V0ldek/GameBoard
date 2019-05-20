using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public static class DescriptionClassExtension
    {
        public static DescriptionTabDto ToDescriptionTabDto(this DescriptionTab descriptionTab) =>
            descriptionTab != null ? new DescriptionTabDto(descriptionTab.Description) : null;

        public static void EditDescriptionTab(
            this DescriptionTab descriptionTab,
            EditDescriptionTabDto editDescriptionTab) => descriptionTab.Description = editDescriptionTab.Description;
    }
}