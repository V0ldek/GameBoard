using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEvent
{
    internal static class GameEventDtoExtensions
    {
        public static GameEventViewModel ToViewModel(this GameEventDto gameEventDto, GameEventPermission permission)
            => new GameEventViewModel(
                gameEventDto.GameEventId,
                gameEventDto.GameEventName,
                gameEventDto.Place,
                gameEventDto.MeetingTime,
                permission,
                gameEventDto.Games,
                gameEventDto.Creator.ToViewModel(),
                gameEventDto.Invitees.Select(u => u.ToViewModel()),
                gameEventDto.Participants.Select(u => u.ToViewModel()));
    }
}