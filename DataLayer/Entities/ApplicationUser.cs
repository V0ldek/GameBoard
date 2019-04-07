using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GameBoard.DataLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Friendship> SentRequests { get; set; }
        public ICollection<Friendship> ReceivedRequests { get; set; }
    }
}
