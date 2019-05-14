using System;
using System.Collections.Generic;
using System.Linq;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;
using GameBoard.LogicLayer.UserSearch.Dtos;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class GameEventDto
    {
        [NotNull] [ItemNotNull] public IEnumerable<string> Games;

        [NotNull] [ItemNotNull] public IEnumerable<UserDto> Invitees;

        [NotNull] [ItemNotNull] public IEnumerable<UserDto> Participants;

        [NotNull] public DescriptionTabDto DescriptionTab { get; }

        public int Id { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? Date { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        public UserDto Creator { get; }

        internal GameEventDto(
            int id,
            [NotNull] string name,
            [CanBeNull] DateTime? date,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games,
            [NotNull] UserDto creator,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> invitees,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> participants,
            [CanBeNull] DescriptionTabDto descriptionTab
        )
        {
            Id = id;
            Name = name;
            Date = date;
            Place = place ?? string.Empty;
            Games = games;
            Creator = creator;
            Invitees = invitees;
            Participants = participants;
            DescriptionTab = descriptionTab;
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