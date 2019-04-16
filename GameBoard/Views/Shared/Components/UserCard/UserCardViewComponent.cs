using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserCard
{
    public class UserCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UserViewModel user, bool miniature = false) =>
            View(miniature ? "UserCardMini" : "UserCard", user);
    }
}