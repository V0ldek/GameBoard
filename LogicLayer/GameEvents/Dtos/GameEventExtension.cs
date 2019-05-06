using System.Collections.Generic;
using System.Linq;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public static class GameEventExtension
    {
        private static IEnumerable<UserDto> GetUsersWithParticipationStatus(
            this GameEvent gameEvent,
            ParticipationStatus participationStatus) =>
            gameEvent.Participations
                .Where(p => p.ParticipationStatus == participationStatus)
                .Select(p => p.Participant.ToDto());

        public static GameEventDto ToGameEventDto(this GameEvent gameEvent) =>
            new GameEventDto(
                gameEvent.Id,
                gameEvent.Name,
                gameEvent.Date,
                gameEvent.Place,
                gameEvent.Games
                    .Where(g => g.PositionOnTheList != null)
                    .OrderBy(g => g.PositionOnTheList)
                    .Select(g => g.Name),
                gameEvent.GetUsersWithParticipationStatus(ParticipationStatus.Creator).Single(),
                gameEvent.GetUsersWithParticipationStatus(ParticipationStatus.PendingGuest),
                gameEvent.GetUsersWithParticipationStatus(ParticipationStatus.AcceptedGuest)
            );

        public static GameEventListItemDto ToGameEventListItemDto(this GameEvent gameEvent) =>
            new GameEventListItemDto(
                gameEvent.Id,
                gameEvent.Name,
                gameEvent.GetUsersWithParticipationStatus(ParticipationStatus.Creator).Single()
            );
    }
}