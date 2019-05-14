
using System.Collections.Generic;
using GameBoard.Models.Groups;
using GameBoard.Models.User;

namespace GameBoard.Models.FriendsSidebar
{
    public class FriendsSidebarViewModel
    {
        public IEnumerable<GroupViewModel> Groups { get; }
        public IEnumerable<UserViewModel> Users { get; }

        public FriendsSidebarViewModel(IEnumerable<GroupViewModel> groups, IEnumerable<UserViewModel> users)
        {
            Groups = groups;
            Users = users;
        }
    }
}
