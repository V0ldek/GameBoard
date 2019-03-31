using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.Friends.Dtos;
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
        public async Task CreateFriendRequest(string userNameFrom, string userNameTo)
        {
            var createFriendRequestDto = new CreateFriendRequestDto(userNameFrom, userNameTo);

            await _friendsService.SendFriendRequestAsync(createFriendRequestDto);
        }
    }
}