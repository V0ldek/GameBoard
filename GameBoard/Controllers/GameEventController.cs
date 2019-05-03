using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.GameEvent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult CreateGameEvent()
        {
            var createGameEventViewModel = new CreateGameEventViewModel
            {
                CreatorUserName = User.Identity.Name
            };

            return View("CreateGameEvent", createGameEventViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateGameEvent(CreateGameEventViewModel createGameEventViewModel)
        {
            await _gameEventService.CreateGameEventAsync(createGameEventViewModel.ToDto());

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult IsGameListValid(string games)
        {
            var normalizedGames = CreateGameEventViewModel.NormalizeGameList(games).ToList();

            if (normalizedGames.Count == 0)
            {
                return Json("You need to specify at least one game.");
            }

            if (normalizedGames.Any(g => g.Length > CreateGameEventViewModel.MaxGameStringLength))
            {
                return Json($"Maximal length for one game is {CreateGameEventViewModel.MaxGameStringLength}.");
            }

            return Json(true);
        }
    }
}
