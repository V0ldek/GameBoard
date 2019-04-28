using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameBoard.DataLayer.Entities
{
    public class GameEvent
    {
        public string Id { get; set; }
        public string EventName { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Place { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<GameEventParticipation> Participations { get; set; }
    }
}
