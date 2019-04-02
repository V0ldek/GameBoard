using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoard.Models.FriendRequest
{
    public class FriendRequestViewModel
    {
        public string Id { get; }
        
        public string UserNameFrom { get; }

        public FriendRequestViewModel(string id, string userNameFrom)
        {
            Id = id;
            UserNameFrom = userNameFrom;
        }
    }
}
