using System;
using System.Collections.Generic;

namespace GameBoard.DataLayer.Entities
{
    public class GameEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? MeetingTime { get; set; } //Not sure if it is safe because we use a converter
        public string Place { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<GameEventParticipation> Participations { get; set; }
    }
}
