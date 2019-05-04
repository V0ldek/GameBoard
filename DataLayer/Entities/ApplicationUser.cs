using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace GameBoard.DataLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Friendship> SentRequests { get; set; }
        public ICollection<Friendship> ReceivedRequests { get; set; }

        public static Expression<Func<ApplicationUser, bool>> UserNameEquals(string userName) =>
            u => u.NormalizedUserName == userName.ToUpper();
    }
}