using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchBox
{
    public class UserSearchBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("UserSearchBox");
    }
}