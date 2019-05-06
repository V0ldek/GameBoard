using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        [NotNull] [ItemNotNull] public IEnumerable<string> Games;

        public int Id { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        public EditGameEventDto(
            int id,
            [NotNull] string name,
            [CanBeNull] DateTime? meetingTime,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games)
        {
            Id = id;
            Name = name;
            MeetingTime = meetingTime;
            Place = place;
            Games = games;
        }
    }
}