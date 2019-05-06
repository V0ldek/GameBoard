using System;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends;
using GameBoard.Models.FriendsSidebar;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.FriendsSidebar
{
    public class FriendsSidebarViewComponent : ViewComponent
    {
        private readonly IFriendsService _friendsService;

        public FriendsSidebarViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;
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

            var friends = await _friendsService.GetFriendsByUserNameAsync(User.Identity.Name);

            return View(
                "FriendsSidebar",
                new FriendsSidebarViewModel(
                    friends.Select(u => u.ToViewModel()),
                    toggled,
                    subComponentName,
                    subComponentArgumentsProvider));
        }
    }
}