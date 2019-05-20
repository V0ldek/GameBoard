using System.Collections.Generic;
using GameBoard.Models.User;

namespace GameBoard.Models.FriendSearch
{
    public class FriendSearchResultViewModel
    {
        public IEnumerable<UserViewModel> Users { get; }
        public string Id { get; }

        public FriendSearchResultViewModel(IEnumerable<UserViewModel> users, string groupId)
        {
            Users = users;
            Id = groupId;
        }
    }
}