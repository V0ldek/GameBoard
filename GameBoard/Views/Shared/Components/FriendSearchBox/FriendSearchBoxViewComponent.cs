using GameBoard.Models.Groups;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.FriendSearchBox
{
    public class FriendSearchBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(GroupViewModel group) =>
            View("FriendsSearchBox", group);
    }
}