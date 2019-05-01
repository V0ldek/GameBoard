using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.GameEvent;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    public class GameEventController : Controller
    {
        private readonly IGameEventService _gameEventService;

        public GameEventController(IGameEventService gameEventService)
        {
            _gameEventService = gameEventService;
        }

        [HttpGet]
        public async Task<IActionResult> GameEvent(int id)
        {
            var gameEvent = await _gameEventService.GetGameEventAsync(id);

            if (gameEvent == null)
            {
                return this.Error("Error!", "Game event not found.", HttpStatusCode.NotFound);
            }

            var userName = User.Identity.Name;
            var permission = gameEvent.GetUserPermission(userName);

            switch (permission)
            {
                case GameEventPermission.AcceptedInvitation:
                case GameEventPermission.Creator:
                case GameEventPermission.PendingInvitation:
                    return View("GameEvent", gameEvent.ToViewModel(permission));
                case GameEventPermission.Forbidden:
                    return this.AccessDenied();
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected permission value {permission}.");
            }
        }
    }
}
