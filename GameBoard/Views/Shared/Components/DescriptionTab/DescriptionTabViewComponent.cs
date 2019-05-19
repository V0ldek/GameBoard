using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchBox
{
    public class DescriptionTabViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("DescriptionTab");
    }
}