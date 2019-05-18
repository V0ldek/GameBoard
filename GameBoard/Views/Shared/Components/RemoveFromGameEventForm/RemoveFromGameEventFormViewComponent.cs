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
        public IViewComponentResult Invoke(int id, string name, string userName) =>
            View("RemoveFromGameEventForm", new RemoveFromGameEventViewModel
            {
                Id = id,
                Name = name,
                UserName = userName
            });
    }
}

