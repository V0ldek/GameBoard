using System;
using System.Collections.Generic;

namespace GameBoard.DataLayer.Entities
{
    public class GameEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Place { get; set; }

        public ICollection<Game> Games { get; set; }

        public DescriptionTab Description { get; set; }

        public ICollection<GameEventParticipation> Participations { get; set; }
    }
}