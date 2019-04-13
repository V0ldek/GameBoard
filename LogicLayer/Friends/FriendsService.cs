using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Parsing;

namespace GameBoard.LogicLayer.Friends
{
    internal sealed class FriendsService : IFriendsService
    {
        private readonly IGameBoardRepository _repository;
        private readonly ILogger _logger;

        public FriendsService(IGameBoardRepository repository, ILogger<FriendsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        {
            string normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers.Where(u => u.NormalizedUserName == normalizedUserName); // is it possible that this user doesn't exist somehow?

            // How does Cascade work? Does it delete user and all of its friendship in a single transaction or something like that?

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

            var allFriends = await friendsInvitedByMe // can I avoid using await here?
                .Union(friendsThatInvitedMe)
                .OrderBy(u => u.UserName) // maybe order by NormalizedUserName? However, I cannot do that with UserDto.
                .ToListAsync();

            return allFriends;
        }

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            var normalizedUserNameFrom = friendRequest.UserNameFrom.ToUpper();
            var normalizedUserNameTo = friendRequest.UserNameTo.ToUpper();

            var requestedById = await _repository.ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserNameFrom)
                .Select(u => u.Id)
                .SingleAsync(); // OrDefault? Deleted user?

            var requestedToId = await _repository.ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserNameTo)
                .Select(u => u.Id)
                .SingleAsync(); // OrDefault? Deleted user? This should be a separate function.

            // what if user deletes its account in this moment? What happens with inserting new Friendship into database? Abort?

            //var friendship = await _repository.Friendships
            //    .SingleOrDefaultAsync(
            //        f => f.RequestedById == requestedById &&
            //            f.RequestedToId == requestedToId);


            //if (friendship != null)
            //{
            //    switch (friendship.FriendshipStatus)
            //    {
            //        case FriendshipStatus.PendingFriendRequest:
            //            throw new FriendRequestAlreadyPendingException(
            //                "You have already sent this user a friend request.");
            //        case FriendshipStatus.Lasts:
            //            throw new FriendRequestAlreadyFinalizedException("You are already friends.");
            //        case FriendshipStatus.Rejected:
            //            break;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //}

            //// I have to check if RequestedTo have sent a friend request to RequestedBy already. 
            //friendship = await _repository.Friendships
            //    .SingleOrDefaultAsync(
            //        f => f.RequestedById == requestedToId &&
            //            f.RequestedToId == requestedById);

            //if (friendship != null)
            //{
            //    switch (friendship.FriendshipStatus)
            //    {
            //        case FriendshipStatus.PendingFriendRequest:
            //            throw new FriendRequestAlreadyPendingException(
            //                $"You have already been invited by this user. Go to {friendRequest.GenerateRequestLink(friendship.Id.ToString())}, to confirm or reject the friend request.");
            //        case FriendshipStatus.Lasts:
            //            throw new FriendRequestAlreadyFinalizedException("You are already friends.");
            //        case FriendshipStatus.Rejected:
            //            break;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //}

            _repository.Friendships.Add(
                new Friendship()
                {
                    RequestedById = requestedById,
                    RequestedToId = requestedToId,
                    FriendshipStatus = FriendshipStatus.PendingFriendRequest
                });

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is SqlException sqlException)
            {
                switch (sqlException.Number)
                {
                    case 50000:
                        throw new FriendRequestAlreadyPendingException(
                            $"You have already been invited by this user. Go to ..., to confirm or reject the friend request.");
                    case 50001:
                        throw new FriendRequestAlreadyPendingException(
                            "You have already sent this user a friend request.");
                    case 50002:
                        throw new FriendRequestAlreadyFinalizedException("You are already friends.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //send email with link
        }

        public Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId)
        {
            var id = int.Parse(friendRequestId);
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
            var id = int.Parse(friendRequestId);
            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == id); //friendship might not exist, because user can delete his account.

            friendship.FriendshipStatus = friendshipStatus;

            await _repository.SaveChangesAsync();
        }

        public async Task AcceptFriendRequestAsync(string friendRequestId) =>
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts); // can I avoid using await here? 

        public async Task RejectFriendRequestAsync(string friendRequestId) =>
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Rejected); // can I avoid using await here?
    }
}
