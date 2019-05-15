using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.Models.GameEvent;
using GameBoard.Models.GameEventInviteForm;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.RemoveFromGameEventForm
{
    public class RemoveFromGameEventFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int gameEventId, string userName) =>
            View("RemoveFromGameEventForm", new RemoveFromGameEventViewModel { GameEventId = gameEventId, UserName = userName }); //TODO:
    }
}

