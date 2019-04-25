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
        public DateTime MeetingTime { get; set; }
        public string Place { get; set; }

        public ApplicationUser Creator { get; set; }
        public string CreatorId { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<GameEventInvitation> Invitations { get; set; }
    }
}
