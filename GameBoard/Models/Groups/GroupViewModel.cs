using GameBoard.Models.User;
using System.Collections.Generic;

namespace GameBoard.Models.Groups
{
    public class GroupViewModel
    {
        public int GroupId { get; }

        public string GroupName { get; }

        public IEnumerable<UserViewModel> Users { get; }

        public GroupViewModel(int groupId, string groupName, IEnumerable<UserViewModel> users)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
        }
    }
}
