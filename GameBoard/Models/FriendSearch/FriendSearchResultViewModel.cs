using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.Models.User;

namespace GameBoard.Models.FriendSearch
{
    public class FriendSearchResultViewModel
    {
        public IEnumerable<UserViewModel> Users { get; }
        public string GroupId { get; }

        public FriendSearchResultViewModel(IEnumerable<UserViewModel> users, string groupId)
        {
            Users = users;
            GroupId = groupId;
        }
    }
}
