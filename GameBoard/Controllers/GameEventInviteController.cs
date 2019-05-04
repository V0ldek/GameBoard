using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents;
using GameBoard.LogicLayer.GameEvents.Dtos;
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
        public async Task<IActionResult> AcceptGameEventInvite(int gameEventId)
        {
            await _gameEventService.AcceptGameEventInvitationAsync(gameEventId, User.Identity.Name);

            return RedirectToAction("GameEvent", "GameEvent", new {id = gameEventId});
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RejectGameEventInvite(int gameEventId)
        {
            await _gameEventService.RejectGameEventInvitationAsync(gameEventId, User.Identity.Name);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateGameEventInvite(int gameEventId, string userName)
        {
            GameEventDto gameEvent;

            try
            {
                gameEvent = await _gameEventService.GetGameEventAsync(gameEventId);
            }
            catch (ApplicationException exception)
            {
                return this.ErrorJson("Error!", exception.Message, HttpStatusCode.BadRequest);
            }
            catch
            {
                return this.ErrorJson(
                    "Error!",
                    "An unexpected error has occured while processing your request.",
                    HttpStatusCode.InternalServerError);
            }

            if (gameEvent == null)
            {
                return this.ErrorJson(
                    "Error!",
                    "Specified game event does not exist.",
                    HttpStatusCode.NotFound);
            }

            if (gameEvent.Creator.UserName != User.Identity.Name)
            {
                return this.ErrorJson(
                    "Error!",
                    "You're unauthorized to perform this action.",
                    HttpStatusCode.Unauthorized);
            }

            try
            {
                await _gameEventService.SendGameEventInvitationAsync(gameEventId, userName);
            }
            catch (ApplicationException exception)
            {
                return this.ErrorJson("Error!", exception.Message, HttpStatusCode.BadRequest);
            }
            catch
            {
                return this.ErrorJson(
                    "Error!",
                    "An unexpected error has occured while processing your request.",
                    HttpStatusCode.InternalServerError);
            }

            return Ok(
                new
                {
                    title = "Invite sent.",
                    message = $"An email with your invitation to the {gameEvent.GameEventName} event has been sent to {userName}."
                });
        }
    }
}