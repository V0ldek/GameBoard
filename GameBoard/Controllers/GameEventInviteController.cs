using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.Configuration;
using GameBoard.Errors;
using GameBoard.LogicLayer.GameEventInvitations;
using GameBoard.LogicLayer.GameEventInvitations.Dtos;
using GameBoard.LogicLayer.GameEvents;
using GameBoard.LogicLayer.GameEvents.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GameBoard.Controllers
{
    [Authorize]
    public class GameEventInvite : Controller
    {
        private readonly IGameEventService _gameEventService;
        private readonly IGameEventInvitationsService _gameEventInvitationsService;
        private readonly HostConfiguration _hostConfiguration;

        public GameEventInvite(
            IGameEventInvitationsService gameEventInvitationsService, 
            IGameEventService gameEventService,
            IOptions<HostConfiguration> hostConfiguration)
        {
            _gameEventInvitationsService = gameEventInvitationsService;
            _gameEventService = gameEventService;
            _hostConfiguration = hostConfiguration.Value;
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

            var createGameEventInvitationDto = new CreateGameEventInvitationDto(
                gameEventId,
                userName,
                eventId => _hostConfiguration.HostAddress + Url.Action(
                    "GameEvent",
                    "gameEvent",
                    new { id = eventId }));

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
                await _gameEventInvitationsService.SendGameEventInvitationAsync(createGameEventInvitationDto);
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