using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.Errors;
using GameBoard.LogicLayer.GameEventInvitations;
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
        private readonly IGameEventInvitationsService _gameEventInvitationsService;

        public GameEventInvite(
            IGameEventInvitationsService gameEventInvitationsService, 
            IGameEventService gameEventService)
        {
            _gameEventInvitationsService = gameEventInvitationsService;
            _gameEventService = gameEventService;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AcceptGameEventInvite(int gameEventId)
        {
            await _gameEventInvitationsService.AcceptGameEventInvitationAsync(gameEventId, User.Identity.Name);

            return RedirectToAction("GameEvent", "GameEvent", new {id = gameEventId});
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RejectGameEventInvite(int gameEventId)
        {
            await _gameEventInvitationsService.RejectGameEventInvitationAsync(gameEventId, User.Identity.Name);

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
                return Error.FromController(this).ErrorJson("Error!", exception.Message, HttpStatusCode.BadRequest);
            }
            catch
            {
                return Error.FromController(this).ErrorJson(
                    "Error!",
                    "An unexpected error has occured while processing your request.",
                    HttpStatusCode.InternalServerError);
            }

            if (gameEvent == null)
            {
                return Error.FromController(this).ErrorJson(
                    "Error!",
                    "Specified game event does not exist.",
                    HttpStatusCode.NotFound);
            }

            if (gameEvent.Creator.UserName != User.Identity.Name)
            {
                return Error.FromController(this).ErrorJson(
                    "Error!",
                    "You're unauthorized to perform this action.",
                    HttpStatusCode.Unauthorized);
            }

            try
            {
                await _gameEventInvitationsService.SendGameEventInvitationAsync(gameEventId, userName);
            }
            catch (ApplicationException exception)
            {
                return Error.FromController(this).ErrorJson("Error!", exception.Message, HttpStatusCode.BadRequest);
            }
            catch
            {
                return Error.FromController(this).ErrorJson(
                    "Error!",
                    "An unexpected error has occured while processing your request.",
                    HttpStatusCode.InternalServerError);
            }

            return Ok(
                new
                {
                    title = "Invite sent.",
                    message =
                        $"An email with your invitation to the {gameEvent.Name} event has been sent to {userName}."
                });
        }
    }
}