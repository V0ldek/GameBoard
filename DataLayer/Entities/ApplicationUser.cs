using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GameBoard.DataLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<FriendRequest> SentRequests { get; set; }
        public ICollection<FriendRequest> ReceivedRequests { get; set; }
    }
}
