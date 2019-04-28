using System;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        private static DateTime _unsetMeetingTime = new DateTime(1, 2, 19);
        [NotNull]
        public int GameEventId { get; }
        public string GameEventName { get; }
        public DateTime MeetingTime { get; }
        public string Place { get; }

        public EditGameEventDto(int gameEventId, string gameEventName = null, DateTime? meetingTime = null, string place = null)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime ?? _unsetMeetingTime;
            Place = gameEventName;
        }

        public bool IsMeetingTimeSet()
        {
            return MeetingTime != _unsetMeetingTime;
        }

    }
}
