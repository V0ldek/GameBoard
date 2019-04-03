using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.Configuration;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.Models.FriendRequest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace GameBoard.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<FriendsController> _logger;
        private readonly HostConfiguration _hostConfiguration;

        public FriendsController(
            IFriendsService friendsService,
            IOptions<HostConfiguration> hostConfiguration,
            ILogger<FriendsController> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
            _hostConfiguration = hostConfiguration.Value;
        }

        [HttpGet]
        public async Task<IActionResult> FriendRequest(string id)
        {
            var friendRequest = await _friendsService.GetFriendRequestAsync(id);

            if (friendRequest == null)
            {
                return this.Error(
                    "Friend request not found",
                    "The friend request you referenced does not exist in the system. " +
                    "Please, make sure the link you followed is identical with the one in the email.",
                    HttpStatusCode.NotFound,
                    _logger);
            }

            if (friendRequest.UserTo.UserName != User.Identity.Name)
            {
                return this.AccessDenied();
            }

            if (friendRequest.Status == FriendRequestDto.FriendRequestStatus.Accepted ||
                friendRequest.Status == FriendRequestDto.FriendRequestStatus.Rejected)
            {
                return this.Error(
                    "Friend request expired",
                    "The friend request you referenced has already been accepted or rejected.",
                    HttpStatusCode.Conflict,
                    _logger);
            }

            return View("FriendRequest", friendRequest.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(string id)
        {
            await _friendsService.AcceptFriendRequestAsync(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(string id)
        {
            await _friendsService.RejectFriendRequestAsync(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest(string userNameFrom, string userNameTo)
        {
            var createFriendRequestDto = new CreateFriendRequestDto(
                userNameFrom,
                userNameTo,
                friendRequestId => _hostConfiguration.HostAddress + Url.Action(
                    "FriendRequest",
                    "Friends",
                    new {id = friendRequestId}));

            try
            {
                await _friendsService.SendFriendRequestAsync(createFriendRequestDto);
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
                    message = $"An email with your invitation has be sent to {userNameTo}." +
                        " Once it's accepted you'll see them in your Friends menu."
                });
        }
    }
}