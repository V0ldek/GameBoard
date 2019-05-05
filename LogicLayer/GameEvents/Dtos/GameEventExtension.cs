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
                .Select(p => p.Participant.ToDto());
        }

        public static GameEventDto ToGameEventDto(this GameEvent gameEvent)
        {
            return new GameEventDto(
                gameEvent.Id,
                gameEvent.Name,
                gameEvent.Date,
                gameEvent.Place,
                gameEvent.Games.Where(g => g.GameStatus == GameStatus.ExistsOnTheList).Select(g => g.Name),
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.Creator).Single(),
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.PendingGuest),
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.AcceptedGuest)
            );
        }

        public static GameEventListItemDto ToGameEventListItemDto(this GameEvent gameEvent)
        {
            return new GameEventListItemDto(
                gameEvent.Id, 
                gameEvent.Name, 
                gameEvent.GetUsersWithSamePartitipationStatus(ParticipationStatus.Creator).Single()
            );
        }
    }
}

