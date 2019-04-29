using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        public int GameEventId { get; }

        [CanBeNull]
        public string GameEventName { get; }

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
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime ?? _unsetMeetingTime;
            Place = gameEventName;
            Games = games;
        }

        public bool IsMeetingTimeSet()
        {
            return MeetingTime != _unsetMeetingTime;
        }

    }
}
