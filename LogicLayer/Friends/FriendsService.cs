using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.Friends.Exceptions;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.Friends
{
    internal sealed class FriendsService : IFriendsService
    {
        private readonly IGameBoardRepository _repository;

        public FriendsService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        {
            var normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers.Where(u => u.NormalizedUserName == normalizedUserName);

            var friendsInvitedByMe = user
                .Include(u => u.SentRequests)
                .SelectMany(u => u.SentRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Select(f => f.RequestedTo);

            var friendsThatInvitedMe = user
                .Include(u => u.ReceivedRequests)
                .SelectMany(u => u.ReceivedRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Select(f => f.RequestedBy);

            var allFriends = await friendsInvitedByMe
                .Union(friendsThatInvitedMe)
                .OrderBy(u => u.NormalizedUserName)
                .ToListAsync();

            return allFriends.ConvertAll(u => u.ToUserDto());
        }

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            if (friendRequest.UserNameTo == friendRequest.UserNameFrom)
            {
                throw new InvitingYourselfException("You cannot invite yourself.");
            }

            var requestedById = await _repository.GetUserIdByUsername(friendRequest.UserNameFrom);
            var requestedToId = await _repository.GetUserIdByUsername(friendRequest.UserNameTo);

            _repository.Friendships.Add(
                new Friendship
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
                            "You have already been invited by this user. Check your inbox for the friend request.");
                    case 50001:
                        throw new FriendRequestAlreadyPendingException(
                            "You have already sent this user a friend request.");
                    case 50002:
                        throw new FriendRequestAlreadyFinalizedException("You are already friends.");
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Not caught SQL Exception with error number {sqlException.Number.ToString()} occured.");
                }
            }

            // TODO: send email with link
        }

        public async Task<FriendRequestDto> GetFriendRequestAsync(int friendRequestId)
        {
            var friendship = await _repository.Friendships.SingleOrDefaultAsync(f => f.Id == friendRequestId);

            return friendship?.ToFriendRequestDto();
        }

        public Task AcceptFriendRequestAsync(int friendRequestId) =>
            ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts);

        public Task RejectFriendRequestAsync(int friendRequestId) =>
            ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Rejected);

        private async Task ChangeFriendRequestStatus(int friendRequestId, FriendshipStatus friendshipStatus)
        {
            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == friendRequestId);

            friendship.FriendshipStatus = friendshipStatus;

            await _repository.SaveChangesAsync();
        }
    }
}