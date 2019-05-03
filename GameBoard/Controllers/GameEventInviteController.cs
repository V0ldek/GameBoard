using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    [Authorize]
    public class GameEventInvite : Controller
    {
        private readonly IGameEventService _gameEventService;

        public GameEventInvite(IGameEventService gameEventService)
        {
            _gameEventService = gameEventService;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AcceptGameEventInvite(int id)
        {
            await _gameEventService.AcceptGameEventInvitationAsync(id, User.Identity.Name);

            return RedirectToAction("GameEvent", "GameEvent", new {id});
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RejectGameEventInvite(int id)
        {
            await _gameEventService.RejectGameEventInvitationAsync(id, User.Identity.Name);

            return RedirectToAction("Index", "Home");
        }
    }
}
