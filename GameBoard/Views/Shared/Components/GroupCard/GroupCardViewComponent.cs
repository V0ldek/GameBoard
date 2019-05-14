using GameBoard.Models.Groups;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GameBoard.Views.Shared.Components.GroupCard
{
    public class GroupCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            int groupId,
            string groupName,
            IEnumerable<UserViewModel> users,
            string subComponentName,
            Func<string, object> subComponentArguments) =>
            View("GroupCard", new GroupViewModel(groupId, groupName, users, subComponentName, subComponentArguments));
    }
}
