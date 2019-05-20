using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.MainTab
{
    public class MainTabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("InfoTab");
    }
}