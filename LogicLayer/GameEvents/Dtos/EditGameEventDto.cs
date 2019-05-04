using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        public int Id { get; }

        [CanBeNull] // I think it should be NotNull.
        public string Name { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games;

        public EditGameEventDto(
            int id, 
            [CanBeNull] string name, 
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
