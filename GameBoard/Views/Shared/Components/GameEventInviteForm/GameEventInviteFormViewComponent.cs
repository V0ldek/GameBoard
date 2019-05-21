using GameBoard.Models.GameEventInviteForm;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GameEventInviteForm
{
    public class GameEventInviteFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(GameEventInviteFormViewModel gameEventInviteFormViewModel) =>
            View("GameEventInviteForm", gameEventInviteFormViewModel);
    }
}