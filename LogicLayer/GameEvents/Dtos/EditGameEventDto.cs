using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        [NotNull]
        public string GameEventId { get;}
        public string GameEventName { get;}
        public DateTime MeetingTime { get; }
        public string Place { get; }

        public EditGameEventDto([NotNull] string gameEventId, string gameEventName, DateTime meetingTime, string place)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime;
            Place = gameEventName;
        }

    }
}
