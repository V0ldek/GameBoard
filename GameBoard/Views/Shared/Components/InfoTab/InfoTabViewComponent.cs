using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchBox
{
    public class InfoTabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("InfoTab");
    }
}