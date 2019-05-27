using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.Friends.Exceptions;
using GameBoard.LogicLayer.Friends.Notifications;
using GameBoard.LogicLayer.Groups;
using GameBoard.LogicLayer.Notifications;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GameBoard.LogicLayer.Friends
{
    internal sealed class FriendsService : IFriendsService
    {
        private readonly IGroupsService _groupsService;
        private readonly INotificationService _notificationService;
        private readonly IGameBoardRepository _repository;
        private readonly GroupsConfiguration _groupsOptions;

        public FriendsService(
            IGameBoardRepository repository,
            INotificationService notificationService,
            IGroupsService groupsService,
            IOptions<GroupsConfiguration> groupsOptions)
        {
            _repository = repository;
            _notificationService = notificationService;
            _groupsService = groupsService;
            _groupsOptions = groupsOptions.Value;
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
        {
            var user = _repository.ApplicationUsers.Where(ApplicationUser.UserNameEquals(userName));

            // EF Core 2.2 cannot translate Unions, so two queries are necessary.
            var friendsInvitedByMe = await user
                .Include(u => u.SentRequests)
                .SelectMany(u => u.SentRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Select(f => f.RequestedTo)
                .ToListAsync();

            var friendsThatInvitedMe = await user
                .Include(u => u.ReceivedRequests)
                .SelectMany(u => u.ReceivedRequests)
                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
                .Select(f => f.RequestedBy)
                .ToListAsync();

            var allFriends = friendsInvitedByMe.Concat(friendsThatInvitedMe).OrderBy(u => u.NormalizedUserName);

            return allFriends.Select(u => u.ToDto());
        }

        public async Task SendFriendRequestAsync(SendFriendRequestDto friendRequest)
        {
            if (friendRequest.UserNameTo == friendRequest.UserNameFrom)
            {
                throw new FriendRequestException("You cannot invite yourself.");
            }

            var userRequestedBy =
                _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(friendRequest.UserNameFrom));
            var userRequestedTo =
                _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(friendRequest.UserNameTo));

            var friendship = new Friendship
            {
                RequestedById = userRequestedBy.Id,
                RequestedToId = userRequestedTo.Id,
                FriendshipStatus = FriendshipStatus.PendingFriendRequest
            };

            using (var transaction = _repository.BeginTransaction())
            {
                await SaveFriendshipAsync(friendship);
                await SendFriendRequestEmailAsync(friendRequest.GenerateRequestLink, friendship);
                transaction.Commit();
            }
        }

        public async Task<FriendRequestDto> GetFriendRequestAsync(int friendRequestId)
        {
            var friendship = await _repository.Friendships
                .Include(f => f.RequestedBy)
                .Include(f => f.RequestedTo)
                .SingleOrDefaultAsync(f => f.Id == friendRequestId);

            return friendship?.ToDto();
        }

        public async Task AcceptFriendRequestAsync(int friendRequestId)
        {
            using (var transaction = _repository.BeginTransaction())
            {
                await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts);

                var friendship = await GetFriendRequestAsync(friendRequestId);

                if (friendship == null)
                {
                    throw new FriendRequestException("The friend request you referenced does not exist in the system.");
                }

                var groupAll = await _groupsService.GetGroupByNamesAsync(
                    friendship.UserFrom.UserName,
                    _groupsOptions.AllFriendsGroupName);
                await _groupsService.AddUserToGroupAsync(friendship.UserTo.UserName, groupAll.Id);

                groupAll = await _groupsService.GetGroupByNamesAsync(
                    friendship.UserTo.UserName,
                    _groupsOptions.AllFriendsGroupName);
                await _groupsService.AddUserToGroupAsync(friendship.UserFrom.UserName, groupAll.Id);

                transaction.Commit();
            }
        }

        public Task RejectFriendRequestAsync(int friendRequestId) =>
            ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Rejected);

        private async Task SaveFriendshipAsync(Friendship friendship)
        {
            _repository.Friendships.Add(friendship);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is SqlException sqlException)
            {
                switch (sqlException.Number)
                {
                    case 50000:
                        throw new FriendRequestException(
                            "You have already been invited by this user. Check your inbox for the friend request.");
                    case 50001:
                        throw new FriendRequestException(
                            "You have already sent this user a friend request.");
                    case 50002:
                        throw new FriendRequestException("You are already friends.");
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Uncaught SQL Exception with error number {sqlException.Number} occured.");
                }
            }
        }

        private Task SendFriendRequestEmailAsync(
            SendFriendRequestDto.RequestLinkGenerator requestLinkGenerator,
            Friendship friendship)
        {
            var notification = new FriendRequestNotification(
                friendship.RequestedBy.UserName,
                friendship.RequestedTo.UserName,
                friendship.RequestedTo.Email,
                requestLinkGenerator(friendship.Id.ToString()));
            return _notificationService.CreateNotificationBatch(notification).SendAsync();
        }

        private async Task ChangeFriendRequestStatus(int friendRequestId, FriendshipStatus friendshipStatus)
        {
            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == friendRequestId);

            if (friendship.FriendshipStatus != FriendshipStatus.PendingFriendRequest)
            {
                throw new InvalidOperationException("You can only change FriendshipStatus of a pending friendship.");
            }

            friendship.FriendshipStatus = friendshipStatus;

            await _repository.SaveChangesAsync();
        }
    }
}