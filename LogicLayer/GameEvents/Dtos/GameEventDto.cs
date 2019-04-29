using System;
using System.Collections.Generic;
using System.Linq;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class GameEventDto
    {
        [NotNull]
        public int GameEventId { get; }

        [NotNull]
        public string GameEventName { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games;

        [NotNull]
        public UserDto Creator { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<UserDto> Invitees;

        [NotNull]
        [ItemNotNull]
        public IEnumerable<UserDto> Participants;

        public GameEventPermission GetUserPermission([NotNull] string userName)
        {
            if (Creator.UserName == userName)
            {
                return GameEventPermission.Creator;
            }
            if (Participants.Any(participant => participant.UserName == userName))
            {
                return GameEventPermission.AcceptedInvitation;
            }
            if (Invitees.Any(invitee => invitee.UserName == userName))
            {
                return GameEventPermission.PendingInvitation;
            }

            return GameEventPermission.Forbidden;
        }

        internal GameEventDto(
            int gameEventId,
            [NotNull] string gameEventName,
            [CanBeNull] DateTime? meetingTime,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games,
            [NotNull] UserDto creator,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> invitees,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> participants)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime;
            Place = place ?? string.Empty;
            Games = games;
            Creator = creator;
            Invitees = invitees;
            Participants = participants;
        }

    }
}
