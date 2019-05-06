using System.Linq;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEvent
{
    internal static class GameEventDtoExtensions
    {
        public static GameEventViewModel ToViewModel(this GameEventDto gameEventDto, GameEventPermission permission) =>
            new GameEventViewModel(
                gameEventDto.Id,
                gameEventDto.Name,
                gameEventDto.Place,
                gameEventDto.Date,
                permission,
                gameEventDto.Games,
                gameEventDto.Creator.ToViewModel(),
                gameEventDto.Invitees.Select(u => u.ToViewModel()),
                gameEventDto.Participants.Select(u => u.ToViewModel()));

        public static EditGameEventViewModel ToEditViewModel(this GameEventDto gameEventDto) =>
            new EditGameEventViewModel
            {
                Id = gameEventDto.Id,
                Name = gameEventDto.Name,
                Date = gameEventDto.Date,
                Place = gameEventDto.Place,
                Games = string.Join('\n', gameEventDto.Games)
            };
    }
}