using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameBoard.DataLayer.Entities
{
    class GameEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public DateTime MeetingTime { get; set; }

        public string Place { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<GameEventInvitation> Invitations { get; set; }
    }
}
