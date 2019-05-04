using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEventList
{
    internal static class GameEventListItemDtoExtensions
    {
        public static GameEventListItemViewModel ToViewModel(this GameEventListItemDto gameEventListItemDto) =>
            new GameEventListItemViewModel(
                gameEventListItemDto.Id,
                gameEventListItemDto.Name,
                gameEventListItemDto.Creator.ToViewModel());
    }
}