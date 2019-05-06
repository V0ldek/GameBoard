using System;
using System.Collections.Generic;
using System.Linq;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class GameEventDto
    {
        [NotNull] [ItemNotNull] public IEnumerable<string> Games;

        [NotNull] [ItemNotNull] public IEnumerable<UserDto> Invitees;

        [NotNull] [ItemNotNull] public IEnumerable<UserDto> Participants;

        public int Id { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        public UserDto Creator { get; }

        internal GameEventDto(
            int id,
            [NotNull] string name,
            [CanBeNull] DateTime? meetingTime,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games,
            [NotNull] UserDto creator,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> invitees,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> participants)
        {
            Id = id;
            Name = name;
            MeetingTime = meetingTime;
            Place = place ?? string.Empty;
            Games = games;
            Creator = creator;
            Invitees = invitees;
            Participants = participants;
        }

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
    }
}