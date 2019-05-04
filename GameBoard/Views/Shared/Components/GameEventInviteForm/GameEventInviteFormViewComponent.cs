using GameBoard.Models.GameEventInviteForm;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GameEventInviteForm
{
    public class GameEventInviteFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int gameEventId, string userName) =>
            View("GameEventInviteForm", new GameEventInviteFormViewModel(gameEventId, userName));
    }
}