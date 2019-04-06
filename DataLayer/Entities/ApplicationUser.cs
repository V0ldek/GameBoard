using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GameBoard.DataLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Friendship> SmallerIdInFriendships { get; set; }
        public ICollection<Friendship> GreaterIdInFriendships { get; set; }
    }
}
