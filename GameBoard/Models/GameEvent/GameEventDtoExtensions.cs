using System.Linq;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEvent
{
    internal static class GameEventDtoExtensions
    {
        public static GameEventViewModel ToViewModel(this GameEventDto gameEventDto, GameEventPermission permission) =>
            new GameEventViewModel(
                gameEventDto.GameEventId,
                gameEventDto.GameEventName,
                gameEventDto.Place,
                gameEventDto.MeetingTime,
                permission,
                gameEventDto.Games,
                gameEventDto.Creator.ToViewModel(),
                gameEventDto.Invitees.Select(u => u.ToViewModel()),
                gameEventDto.Participants.Select(u => u.ToViewModel()));

        public static EditGameEventViewModel ToEditViewModel(this GameEventDto gameEventDto) =>
            new EditGameEventViewModel
            {
                Id = gameEventDto.GameEventId,
                Name = gameEventDto.GameEventName,
                Date = gameEventDto.MeetingTime,
                Place = gameEventDto.Place,
                Games = string.Join('\n', gameEventDto.Games)
            };
    }
}