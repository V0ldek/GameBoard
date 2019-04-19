using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEventLogic.Dtos
{
    public sealed class CreateGameEventDto
    {
        [NotNull]
        public string UserId { get; }

        [NotNull]
        //Forma stringa ma postać "yyyy-MM-dd HH:mm"
        public string MeetingTime { get; }

        public string Place { get; }

        public CreateGameEventDto ([NotNull] string userId, [NotNull] string meetingTime, string place)
        {
            UserId = userId;
            MeetingTime = meetingTime;
            Place = place ?? string.Empty ;
        }
    }
}
