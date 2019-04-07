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
using GameBoard.LogicLayer.Friends.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.Friends
{
    internal /*sealed?*/ class FriendService : IFriendsService
    {
        private readonly IGameBoardRepository _repository;

        public FriendService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        {
            // I should change naming if I decide to stick to Friendship with requestedBy and requestedTo.
            
            var user = _repository.ApplicationUsers.Where(u => u.UserName == userName);

            var smallerIdThanFriends = user
                .Include(u => u.SentRequests)
                .SelectMany(u => u.SentRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedTo)
                .Select(f => new UserDto(f.RequestedTo.Id, f.RequestedTo.UserName, f.RequestedTo.Email));

            var greaterIdThanFriends = user
                .Include(u => u.ReceivedRequests)
                .SelectMany(u => u.ReceivedRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedBy)
                .Select(f => new UserDto(f.RequestedBy.Id, f.RequestedBy.UserName, f.RequestedBy.Email));

            var allFriends = await smallerIdThanFriends
                .Union(greaterIdThanFriends)
                .OrderBy(u => u.UserName)
                .ToListAsync(); // can I avoid using await here?

            return allFriends;
        }

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            //using (var transaction = new GameBoardDbContext())
            {
                var friendship = await _repository.Friendships
                    .SingleOrDefaultAsync(
                        f => f.RequestedById == friendRequest.UserNameFrom &&
                            f.RequestedToId == friendRequest.UserNameTo); // wrong id mixed up with userName


                if (friendship != null)
                {
                    switch (friendship.FriendshipStatus)
                    {
                        case FriendshipStatus.PendingFriendRequest:
                            throw new FriendRequestAlreadyPendingException("You have already sent him a friend request.");
                        case FriendshipStatus.Lasts:
                            throw new FriendRequestAlreadyFinalizedException("You are already friends.");
                        case FriendshipStatus.Rejected:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                friendship = await _repository.Friendships
                    .SingleOrDefaultAsync(
                        f => f.RequestedById == friendRequest.UserNameTo &&
                            f.RequestedToId == friendRequest.UserNameFrom); // wrong id mixed up with userName

                if (friendship != null)
                {
                    switch (friendship.FriendshipStatus)
                    {
                        case FriendshipStatus.PendingFriendRequest:
                            throw new FriendRequestAlreadyPendingException("You have already been invited. Go to ...here should be link..., to confirm or reject friend request.");
                        case FriendshipStatus.Lasts:
                            throw new FriendRequestAlreadyFinalizedException("You are already friends.");
                        case FriendshipStatus.Rejected:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                _repository.Friendships.Add(
                    new Friendship()
                    {
                        //auto increment
                        RequestedById = friendRequest.UserNameFrom,
                        RequestedToId = friendRequest.UserNameTo,
                        FriendshipStatus = FriendshipStatus.PendingFriendRequest
                    });

                await _repository.SaveChangesAsync();

                // commit transaction
                // what happens if an exception is thrown within the transaction?

            }

            //send email with link
        }

        public Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId)
        {
            int id = Int32.Parse(friendRequestId);
            var friendship = _repository.Friendships.Where(f => f.Id == id);

            return friendship
                .Include(f => f.RequestedBy)
                .Include(f => f.RequestedTo)
                .Select(
                    f => new FriendRequestDto(
                        friendRequestId,
                        new UserDto(f.RequestedBy.Id, f.RequestedBy.UserName, f.RequestedBy.Email),//wrong, greater can send request too.
                        new UserDto(f.RequestedTo.Id, f.RequestedTo.UserName, f.RequestedTo.Email),
                        f.FriendshipStatus.ToFriendRequest()))
                .SingleAsync();
        }

        private async Task ChangeFriendRequestStatus(string friendRequestId, FriendshipStatus friendshipStatus)
        {
            int id = Int32.Parse(friendRequestId);
            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == id); // can I update in single step?

            friendship.FriendshipStatus = friendshipStatus;

            _repository.Friendships.Update(friendship);
            await _repository.SaveChangesAsync(); // can I avoid using await here?
        }

        public async Task AcceptFriendRequestAsync(string friendRequestId) =>
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts); // can I avoid using await here? 

        public async Task RejectFriendRequestAsync(string friendRequestId) =>
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Rejected); // can I avoid using await here?
    }
}
