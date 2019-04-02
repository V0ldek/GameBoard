using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.Friends.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    public class FriendsController : Controller
    {
        private readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest(string userNameFrom, string userNameTo)
        {
            var createFriendRequestDto = new CreateFriendRequestDto(userNameFrom, userNameTo);

            try
            {
                await _friendsService.SendFriendRequestAsync(createFriendRequestDto);
            }
            catch (ApplicationException exception)
            {
                return BadRequest(
                    new
                    {
                        title = "Error!",
                        message = exception.Message
                    });
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new
                    {
                        title = "Error!",
                        message = "An unexpected error has occured while processing your request."
                    });
            }

            return Ok(
                new
                {
                    title = "Invite sent.",
                    message =
                        $"An email with your invitation has be sent to {userNameTo}. Once it's accepted you'll see them in your Friends menu."
                });
        }
    }
}