﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
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
            var user = _repository.ApplicationUsers.SingleAsync(u => u.UserName == userName);
            var friends = user.Include(u => u.SentRequests).Select(u => u.SentRequests).Where(f => f.F);
            return null;
        }

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            _repository.Friendships.Add(
                new Friendship()
                {
                    //auto increment
                    RequestedById = friendRequest.UserNameFrom,
                    RequestedToId = friendRequest.UserNameTo,
                    FriendshipStatus = FriendshipStatus.PendingFriendRequest
                });

            // do I have to add this friendship to ApplicationUsers collections?

            // send mail with link
        }

        public async Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId)
        {
            int id = Int32.Parse(friendRequestId); // can throw exception

            var friendship = _repository.Friendships.Where(f => f.Id == id);

            var userFrom = friendship.Include(f => f.RequestedToId)
                .AsEnumerable()
                .Select(f => (f.RequestedTo.Id, f.RequestedTo.UserName, f.RequestedTo.Email));

            //return new FriendRequestDto(friendRequestId, friendship.RequestedBy, friendship.RequestedTo, /* friendshipStatus */ FriendRequestDto.FriendRequestStatus.Sent);
        }

        public async Task AcceptFriendRequestAsync(string friendRequestId)
        {
            throw new NotImplementedException();
        }

        public async Task RejectFriendRequestAsync(string friendRequestId)
        {
            throw new NotImplementedException();
        }
    }
}
