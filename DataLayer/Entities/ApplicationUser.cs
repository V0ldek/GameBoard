using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GameBoard.DataLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Friendship> SentRequests { get; set; }
        public ICollection<Friendship> ReceivedRequests { get; set; }

        public ICollection<GameEventParticipation> Participations { get; set; }
    }
}