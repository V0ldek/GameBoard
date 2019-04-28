using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class CreateGameEventDto
    {
        [NotNull]
        public string CreatorId { get; }

        [NotNull]
        public string GameEventName { get; }

        public DateTime MeetingTime { get; } // It is not specified in the user story and I know we have a separate user story concerning this feature, but it won't work out most likely, so maybe we should put this in here for now.

        public string Place { get; }

        public CreateGameEventDto(
            [NotNull] string creatorId,
            [NotNull] string gameEventName,
            DateTime meetingTime,
            string place)
        {
            CreatorId = creatorId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime;
            Place = place;
        }
    }
}
