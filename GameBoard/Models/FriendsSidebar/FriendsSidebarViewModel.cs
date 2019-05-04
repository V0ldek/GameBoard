using System;
using System.Collections.Generic;
using GameBoard.Models.User;

namespace GameBoard.Models.FriendsSidebar
{
    public class FriendsSidebarViewModel
    {
        public IEnumerable<UserViewModel> Friends { get; }

        public bool Toggled { get; }

        public string SubComponentName { get; }

        public Func<string, object> SubComponentArguments { get; }

        public FriendsSidebarViewModel(
            IEnumerable<UserViewModel> friends,
            bool toggled,
            string subComponentName,
            Func<string, object> subComponentArguments)
        {
            Friends = friends;
            Toggled = toggled;
            SubComponentName = subComponentName;
            SubComponentArguments = subComponentArguments;
        }
    }
}