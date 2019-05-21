using GameBoard.Models.GameEvent;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.RemoveFromGameEventForm
{
    public class RemoveFromGameEventFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(RemoveFromGameEventViewModel removeFromGameEventViewModel) =>
            View("RemoveFromGameEventForm", removeFromGameEventViewModel);
    }
}