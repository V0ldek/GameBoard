using System;
using System.Collections.Generic;
using GameBoard.Models.Groups;

namespace GameBoard.Models.FriendsSidebar
{
    public class FriendsSidebarViewModel
    {
        public IEnumerable<GroupViewModel> Groups { get; }

        public bool Toggled { get; }

        public string SubComponentName { get; }

        public Func<string, object> SubComponentArguments { get; }

        public FriendsSidebarViewModel(
            IEnumerable<GroupViewModel> groups,
            bool toggled,
            string subComponentName,
            Func<string, object> subComponentArguments)
        {
            Groups = groups;
            Toggled = toggled;
            SubComponentName = subComponentName;
            SubComponentArguments = subComponentArguments;
        }
    }
}