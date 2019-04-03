/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.UserSearch.Dtos;
using GameBoard.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.Friends
{
    class FriendService : IFriendsService
    {
        private readonly IGameBoardRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendService(IGameBoardRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        {
            var user = _repository.ApplicationUsers.Where(u => u.UserName == userName);
            var friends = user.Include(u => u.SentRequests).Where();
            return null;
        }

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest) => throw new NotImplementedException();

        public async Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId) => throw new NotImplementedException();

        public async Task AcceptFriendRequestAsync(string friendRequestId) => throw new NotImplementedException();

        public async Task RejectFriendRequestAsync(string friendRequestId) => throw new NotImplementedException();
    }
}*/
