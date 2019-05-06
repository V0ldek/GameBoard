using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.Groups;
using GameBoard.Models.Groups;
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("FriendsSidebarUnauthenticated");
            }

            var groups = await _groupsService.GetGroupsByUserNameAsync(User.Identity.Name);

            return View("FriendsSidebar", groups.Select(u => u.ToViewModel()));
        }
    }
}