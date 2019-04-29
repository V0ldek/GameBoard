using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEventList
{
    internal static class GameEventListItemDtoExtensions
    {
        public static GameEventListItemViewModel ToViewModel(this GameEventListItemDto gameEventListItemDto) =>
            new GameEventListItemViewModel(
                gameEventListItemDto.GameEventId,
                gameEventListItemDto.GameEventName,
                gameEventListItemDto.Creator.ToViewModel());
    }
}