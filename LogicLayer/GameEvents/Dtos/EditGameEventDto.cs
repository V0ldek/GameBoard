using System;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        [NotNull]
        public int GameEventId { get; }
        public string GameEventName { get; }
        public DateTime MeetingTime { get; }
        public string Place { get; }

        public EditGameEventDto([NotNull] int gameEventId, string gameEventName, DateTime meetingTime, string place)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime;
            Place = gameEventName;
        }

    }
}
