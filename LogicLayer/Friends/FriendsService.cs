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
using Microsoft.Extensions.Logging;

namespace GameBoard.LogicLayer.Friends
{
    internal /*sealed?*/ class FriendsService : IFriendsService
    {
        private readonly IGameBoardRepository _repository;
        private readonly ILogger _logger;

        public FriendsService(IGameBoardRepository repository, ILogger<FriendsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //public Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        //{
        //    _logger.LogInformation("tralalalalalalalsdlasdladslasdlasddddsssssssssssssssssssssssssssssssssssssss0ds;fd;d");
        //    return Task.FromResult(
        //        new List<UserDto>
        //        {
        //            new UserDto("1", "Alice", "alice@gmail.com"),
        //            new UserDto("2", "V0ldek", "registermen@gmail.com"),
        //            new UserDto("3", "Bob", "bob@gmail.com"),
        //        } as IEnumerable<UserDto>);
        //    //: Task.FromResult((IEnumerable<UserDto>)null);
        //}

        public async Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        {
            var user = _repository.ApplicationUsers.Where(u => u.UserName == userName);

            var friendsInvitedByMe = user
                .Include(u => u.SentRequests)
                .SelectMany(u => u.SentRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedTo)
                .Select(f => new UserDto(f.RequestedTo.Id, f.RequestedTo.UserName, f.RequestedTo.Email))
                .DefaultIfEmpty();

            var friendsThatInvitedMe = user
                .Include(u => u.ReceivedRequests)
                .DefaultIfEmpty()
                .SelectMany(u => u.ReceivedRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedBy)
                .Select(f => new UserDto(f.RequestedBy.Id, f.RequestedBy.UserName, f.RequestedBy.Email));

            _logger.LogInformation("before execution");

            //var allFriends = await friendsInvitedByMe
            //    .Union(friendsThatInvitedMe)
            //    .OrderBy(u => u.UserName)
            //    .ToListAsync(); // can I avoid using await here?

            var friends = await friendsInvitedByMe.ToListAsync();

            _logger.LogInformation("after execution");

            return friends;

            //return allFriends;
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
            var friendship = _repository.Friendships.Where(f => f.Id == id); //friendship might not exist, because user can delete his account.

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
