using GameBoard.Models.User;
using System;
using System.Collections.Generic;

namespace GameBoard.Models.Groups
{
    public class GroupViewModel
    {
        public int GroupId { get; }

        public string GroupName { get; }

        public IEnumerable<UserViewModel> Users { get; }

        public string SubComponentName { get; }

        public Func<string, object> SubComponentArguments { get; }

        public GroupViewModel(int groupId, string groupName, IEnumerable<UserViewModel> users, string subComponentName,
            Func<string, object> subComponentArguments)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
            SubComponentName = subComponentName;
            SubComponentArguments = subComponentArguments;
        }

        public GroupViewModel(int groupId, string groupName, IEnumerable<UserViewModel> users)
        {
            GroupId = groupId;
            GroupName = groupName;
            Users = users;
        }
    }
}
