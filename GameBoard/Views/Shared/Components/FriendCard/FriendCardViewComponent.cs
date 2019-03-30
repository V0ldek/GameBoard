using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.FriendCard
{
    public class FriendCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UserViewModel friend) => View("FriendCard", friend);
    }
}
