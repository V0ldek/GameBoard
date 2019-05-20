using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.InfoTab
{
    public class InfoTabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("InfoTab");
    }
}