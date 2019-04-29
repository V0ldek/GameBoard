using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents.Dtos;

namespace GameBoard.Models.GameEventList
{
    internal static class GameEventListDtoExtensions
    {
        public static GameEventListViewModel ToViewModel(this GameEventListDto gameEventListDto) =>
            new GameEventListViewModel(
                gameEventListDto.AcceptedGameEvents.Select(g => g.ToViewModel()),
                gameEventListDto.PendingGameEvents.Select(g => g.ToViewModel()),
                gameEventListDto.CreatedGameEvents.Select(g => g.ToViewModel()));
    }
}