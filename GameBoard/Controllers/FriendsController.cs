using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GameBoard.Configuration;
using GameBoard.Errors;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.UserSearch;
using GameBoard.Models.FriendRequest;
using GameBoard.Models.FriendSearch;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameBoard.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IUserSearchService _userSearchService;
        private readonly IFriendsService _friendsService;
        private readonly HostConfiguration _hostConfiguration;
        private readonly ILogger<FriendsController> _logger;

        public FriendsController(
            IUserSearchService userSearchService,
            IFriendsService friendsService,
            IOptions<HostConfiguration> hostConfiguration,
            ILogger<FriendsController> logger)
        {
            _userSearchService = userSearchService;
            _friendsService = friendsService;
            _logger = logger;
            _hostConfiguration = hostConfiguration.Value;
        }

        [HttpGet]
        public async Task<IActionResult> SearchFriendsForGroup(string userName, string groupId, string input)
        {
            var friends = await _friendsService.GetFriendsByUserNameAsync(userName);
            friends = friends.Where(x => x.UserName.ToUpper().Contains(input.ToUpper()));
            var model = new FriendSearchResultViewModel(friends.Select(u => u.ToViewModel()), Convert.ToInt32(groupId));
            return ViewComponent("FriendSearchResults", model);
        }

        [HttpGet]
        public async Task<IActionResult> FriendRequest(int id)
        {
            var friendRequest = await _friendsService.GetFriendRequestAsync(id);

            if (friendRequest == null)
            {
                return Error.FromController(this).Error(
                    "Friend request not found",
                    "The friend request you referenced does not exist in the system. " +
                    "Please, make sure the link you followed is identical with the one in the email.",
                    HttpStatusCode.NotFound,
                    _logger);
            }

            if (friendRequest.UserTo.UserName != User.Identity.Name)
            {
                return Error.FromController(this).AccessDenied();
            }

            if (friendRequest.Status == FriendRequestDto.FriendRequestStatus.Accepted ||
                friendRequest.Status == FriendRequestDto.FriendRequestStatus.Rejected)
            {
                return Error.FromController(this).Error(
                    "Friend request expired",
                    "The friend request you referenced has already been accepted or rejected.",
                    HttpStatusCode.Conflict,
                    _logger);
            }

            return View("FriendRequest", friendRequest.ToViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AcceptFriendRequest(int id)
        {
            await _friendsService.AcceptFriendRequestAsync(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RejectFriendRequest(int id)
        {
            await _friendsService.RejectFriendRequestAsync(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SendFriendRequest(string userNameFrom, string userNameTo)
        {
            var createFriendRequestDto = new SendFriendRequestDto(
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
                    message = $"An email with your invitation has been sent to {userNameTo}." +
                        " Once it's accepted you'll see them in your Friends menu."
                });
        }
    }
}