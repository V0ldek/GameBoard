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
            string normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers.Where(u => u.NormalizedUserName == normalizedUserName); // is it possible that this user doesn't exist somehow.

            var friendsInvitedByMe = user
                .Include(u => u.SentRequests)
                .SelectMany(u => u.SentRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedTo)
                .Select(f => new UserDto(f.RequestedTo.Id, f.RequestedTo.UserName, f.RequestedTo.Email));

            var friendsThatInvitedMe = user
                .Include(u => u.ReceivedRequests)
                .SelectMany(u => u.ReceivedRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Include(f => f.RequestedBy)
                .Select(f => new UserDto(f.RequestedBy.Id, f.RequestedBy.UserName, f.RequestedBy.Email));

            var allFriends = await friendsInvitedByMe
                .Union(friendsThatInvitedMe)
                .OrderBy(u => u.UserName) // maybe order by NormalizedUserName? However, I cannot do that with UserDto.
                .ToListAsync(); // can I avoid using await here?

            return allFriends;
        }

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            var normalizedUserNameFrom = friendRequest.UserNameFrom;
            var normalizedUserNameTo = friendRequest.UserNameTo;

            //using (var transaction = new GameBoardDbContext())
            {
                var requestedById = await _repository.ApplicationUsers
                    .Where(u => u.NormalizedUserName == normalizedUserNameFrom)
                    .Select(u => u.Id)
                    .SingleAsync(); // OrDefault? Deleted user?

                var requestedToId = await _repository.ApplicationUsers
                    .Where(u => u.NormalizedUserName == normalizedUserNameTo)
                    .Select(u => u.Id)
                    .SingleAsync(); // OrDefault? Deleted user? This should be a separate function.

                var friendship = await _repository.Friendships
                    .SingleOrDefaultAsync(
                        f => f.RequestedById == requestedById &&
                            f.RequestedToId == requestedToId);


                if (friendship != null)
                {
                    switch (friendship.FriendshipStatus)
                    {
                        case FriendshipStatus.PendingFriendRequest:
                            throw new FriendRequestAlreadyPendingException("You have already sent this user a friend request.");
                        case FriendshipStatus.Lasts:
                            throw new FriendRequestAlreadyFinalizedException("You are already friends.");
                        case FriendshipStatus.Rejected:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // checking if RequestedTo has already sent a friend request to RequestedBy and they have ongoing friendship.  
                friendship = await _repository.Friendships
                    .SingleOrDefaultAsync(
                        f => f.RequestedById == requestedToId &&
                            f.RequestedToId == requestedById); // wrong id mixed up with userName

                if (friendship != null)
                {
                    switch (friendship.FriendshipStatus)
                    {
                        case FriendshipStatus.PendingFriendRequest:
                            throw new FriendRequestAlreadyPendingException("You have already been invited by this user. Go to ...here should be a link..., to confirm or reject the friend request.");
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
                        RequestedById = requestedById,
                        RequestedToId = requestedToId,
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
                        new UserDto(f.RequestedBy.Id, f.RequestedBy.UserName, f.RequestedBy.Email),
                        new UserDto(f.RequestedTo.Id, f.RequestedTo.UserName, f.RequestedTo.Email),
                        f.FriendshipStatus.ToFriendRequest()))
                .SingleAsync();
        }

        private async Task ChangeFriendRequestStatus(string friendRequestId, FriendshipStatus friendshipStatus)
        {
            int id = Int32.Parse(friendRequestId);
            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == id); // can I update in single step?

            friendship.FriendshipStatus = friendshipStatus;

            //_repository.Friendships.Update(friendship);
            await _repository.SaveChangesAsync(); // can I avoid using await here?
        }

        public async Task AcceptFriendRequestAsync(string friendRequestId) =>
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts); // can I avoid using await here? 

        public async Task RejectFriendRequestAsync(string friendRequestId) =>
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Rejected); // can I avoid using await here?
    }
}
