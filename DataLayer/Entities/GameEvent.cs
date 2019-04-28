﻿using System;
using System.Collections.Generic;

namespace GameBoard.DataLayer.Entities
{
    public class GameEvent
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Place { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<GameEventParticipation> Participations { get; set; }
    }
}