using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        public int Id { get; }

        [CanBeNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games;

        public EditGameEventDto(
            int gameEventId, 
            [CanBeNull] string gameEventName, 
            [CanBeNull] DateTime? meetingTime, 
            [CanBeNull] string place, 
            [NotNull] [ItemNotNull] IEnumerable<string> games)
        {
            Id = gameEventId;
            Name = gameEventName;
            MeetingTime = meetingTime;
            Place = gameEventName;
            Games = games;
        }

    }
}
