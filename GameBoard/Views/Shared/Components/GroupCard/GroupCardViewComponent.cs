using System;
using System.Collections.Generic;
using GameBoard.Models.Groups;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GroupCard
{
    public class GroupCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            int groupId,
            string groupName,
            IEnumerable<UserViewModel> users,
            bool groupInviteEnabled,
            int gameEventId,
            string gameEventName,
            string subComponentName,
            Func<string, object> subComponentArguments) =>
            View(
                "GroupCard",
                new GroupViewModel(
                    groupId,
                    groupName,
                    users,
                    groupInviteEnabled,
                    gameEventId,
                    gameEventName,
                    subComponentName,
                    subComponentArguments));
    }
}