using System;
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
            var user = _repository.ApplicationUsers.Where(u => u.UserName == userName);
            var friends = user.Include(u => u.SentRequests)
                .Include(u => u.ReceivedRequests)
                .SelectMany(u => u.SentRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedTo)
                .Select(f => f.RequestedTo.Id /* UserName, Email */)
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

            // do I have to add this friendship to ApplicationUsers' collections?

            await _repository.SaveChangesAsync();

            // send mail with link
        }

        public async Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId)
        {
            int id = Int32.Parse(friendRequestId); // can throw exception
            var friendship = _repository.Friendships.Where(f => f.Id == id);

            var result = friendship
                .Include(f => f.RequestedBy)
                .Include(f => f.RequestedTo)
                .AsEnumerable() //correct?
                .Select(
                    f => (friendshipStatus: f.FriendshipStatus,
                        userFromId: f.RequestedBy.Id,
                        userFromName: f.RequestedBy.UserName,
                        userFromEmail: f.RequestedBy.Email,
                        userToId: f.RequestedTo.Id,
                        userToName: f.RequestedTo.UserName,
                        userToEmail: f.RequestedTo.Email))
                .First(f => true); //correct? Can I assume that friendship with this id exists?

            //return new FriendRequestDto(friendRequestId, friendship.RequestedBy, friendship.RequestedTo, /* friendshipStatus */ FriendRequestDto.FriendRequestStatus.Sent);
        }

        public async Task AcceptFriendRequestAsync(string friendRequestId)
        {
            int id = Int32.Parse(friendRequestId);
            var friendship = _repository.Friendships.First(f => f.Id == id); //Can I assume that friendship with this id exists?

            friendship.FriendshipStatus = FriendshipStatus.Lasts;

            _repository.Friendships.Update(friendship);
            _repository.SaveChangesAsync();
        }

        public async Task RejectFriendRequestAsync(string friendRequestId)
        {
            throw new NotImplementedException();
        }
    }
}
