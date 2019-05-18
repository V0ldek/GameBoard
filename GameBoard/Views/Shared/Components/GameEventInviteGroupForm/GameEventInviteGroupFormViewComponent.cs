using GameBoard.LogicLayer.Groups.Dtos;
using GameBoard.Models.GameEventInviteForm;
using GameBoard.Models.GameEventInviteGroupForm;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GameEventInviteGroupForm
{
    public class GameEventInviteGroupFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int gameEventId, string gameEventName, GroupDto group) =>
            View("GameEventInviteGroupForm", new GameEventInviteFormViewModel(gameEventId, gameEventName, group));
    }
}