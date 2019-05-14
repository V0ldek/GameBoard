using System;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Groups;
using GameBoard.Models.Groups;
using GameBoard.Models.FriendsSidebar;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.FriendsSidebar
{
    public class FriendsSidebarViewComponent : ViewComponent
    {
       private readonly IGroupsService _groupsService;

        public FriendsSidebarViewComponent(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            bool toggled = false,
            string subComponentName = null,
            Func<string, object> subComponentArgumentsProvider = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("FriendsSidebarUnauthenticated");
            }

            var groups = await _groupsService.GetGroupsByUserNameAsync(User.Identity.Name);

            return View(
                "FriendsSidebar",
                new FriendsSidebarViewModel(
                    groups.Select(g => g.ToViewModel()),
                    toggled,
                    subComponentName,
                    subComponentArgumentsProvider));
            //return View("FriendsSidebar", groups.Select(u => u.ToViewModel()));
        }
    }
}