using GameBoard.Models.Groups;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GroupCard
{
    public class FriendSearchBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(GroupViewModel group, bool miniature = false) =>
            View("FriendsSearchBox", group);
    }
}