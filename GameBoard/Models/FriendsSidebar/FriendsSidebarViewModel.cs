using System;
using System.Collections.Generic;
using GameBoard.Models.Groups;

namespace GameBoard.Models.FriendsSidebar
{
    public class FriendsSidebarViewModel
    {
        public IEnumerable<GroupViewModel> Groups { get; }

        public bool Toggled { get; }

        public bool GroupInviteEnabled { get; }

        public int GameEventId { get; }

        public string GameEventName { get; }

        public string SubComponentName { get; }

        public Func<string, object> SubComponentArguments { get; }

        public FriendsSidebarViewModel(
            IEnumerable<GroupViewModel> groups,
            bool toggled,
            bool groupInviteEnabled,
            int gameEventId,
            string gameEventName,
            string subComponentName,
            Func<string, object> subComponentArguments)
        {
            Groups = groups;
            Toggled = toggled;
            GroupInviteEnabled = groupInviteEnabled;
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            SubComponentName = subComponentName;
            SubComponentArguments = subComponentArguments;
        }
    }
}