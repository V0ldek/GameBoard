using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public static class DescriptionClassExtension
    {
        public static DescriptionTabDto ToDescriptionTabDto(this DescriptionTab descriptionTab) =>
            new DescriptionTabDto(descriptionTab.Id, descriptionTab.GameEventId, descriptionTab.Description);
    }
}