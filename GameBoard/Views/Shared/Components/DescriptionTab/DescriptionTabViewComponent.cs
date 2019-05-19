using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.DescriptionTab
{
    public class DescriptionTabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("DescriptionTab");
    }
}