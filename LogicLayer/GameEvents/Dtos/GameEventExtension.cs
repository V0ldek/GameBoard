using System.Collections;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.UserSearch.Dtos;
using System.Linq;
using GameBoard.DataLayer.Enums;
using System.Collections.Generic;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public static class GameEventExtension
    {
        private static IEnumerable<UserDto> GetUsersWithSamePartitipationStatus(
            this GameEvent gameEvent,
            ParticipationStatus participationStatus)
        {
            return gameEvent.Participations
                .Where(p => p.ParticipationStatus == participationStatus)
                .Select(p => p.Paticipant.ToUserDto());
        }

        public static GameEventDto ToGameEventDto(this GameEvent gameEvent)
        {
            return new GameEventDto(
                gameEvent.Id,
                gameEvent.EventName, // strings are immutable so we don't have to create a new instance of the same object by ourselves
                gameEvent.MeetingTime,
                gameEvent.Place, // strings are immutable so we don't have to create a new instance of the same object by ourselves
                gameEvent.Games.Select(g => g.Name), // strings are immutable so we don't have to create a new instance of the same object by ourselves
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.Creator).Single(), //not sure if that is correct. Does include do the job?
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.PendingGuest),
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.AcceptedGuest)
            );
        }

        public static GameEventListItemDto ToGameEventListItemDto(this GameEvent gameEvent)
        {
            return new GameEventListItemDto(
                gameEvent.Id, 
                gameEvent.EventName, 
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.Creator).Single()
            );
        }
    }
}

