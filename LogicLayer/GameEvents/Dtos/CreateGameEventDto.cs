using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class CreateGameEventDto
    {
        [NotNull]
        public string CreatorUserName { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games;

        public CreateGameEventDto(
            [NotNull] string creatorUserName,
            [NotNull] string name,
            [CanBeNull] DateTime? meetingTime,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games)
        {
            CreatorUserName = creatorUserName;
            Name = name;
            MeetingTime = meetingTime;
            Place = place;
            Games = games;
        }
    }
}
