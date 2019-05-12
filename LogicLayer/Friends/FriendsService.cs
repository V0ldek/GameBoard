﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.Friends.Exceptions;
using GameBoard.LogicLayer.Groups;
using GameBoard.LogicLayer.Notifications;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.Friends
{
    internal sealed class FriendsService : IFriendsService
    {
        private readonly IGroupsService _groupsService;
        private readonly IMailSender _mailSender;
        private readonly IGameBoardRepository _repository;

        public FriendsService(IGameBoardRepository repository, IMailSender mailSender, IGroupsService groupsService)
        {
            _repository = repository;
            _mailSender = mailSender;
            _groupsService = groupsService;
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

        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            if (friendRequest.UserNameTo == friendRequest.UserNameFrom)
            {
                throw new InvitingYourselfException("You cannot invite yourself.");
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

            await SaveFriendshipAsync(friendship);
            await SendFriendRequestEmailAsync(friendRequest.GenerateRequestLink, friendship);
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
            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts);

            var friendship = await GetFriendRequestAsync(friendRequestId);

            if (friendship == null)
            {
                throw new NullReferenceException("The friend request you referenced does not exist in the system.");
            }

            var groupAll = await _groupsService.GetGroupByNamesAsync(friendship.UserFrom.UserName, "All");
            await _groupsService.AddUserToGroupAsync(friendship.UserTo.UserName, groupAll.GroupId);

            groupAll = await _groupsService.GetGroupByNamesAsync(friendship.UserTo.UserName, "All");
            await _groupsService.AddUserToGroupAsync(friendship.UserFrom.UserName, groupAll.GroupId);
           
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
                        throw new FriendRequestAlreadyPendingException(
                            "You have already been invited by this user. Check your inbox for the friend request.");
                    case 50001:
                        throw new FriendRequestAlreadyPendingException(
                            "You have already sent this user a friend request.");
                    case 50002:
                        throw new FriendRequestAlreadyFinalizedException("You are already friends.");
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Uncaught SQL Exception with error number {sqlException.Number} occured.");
                }
            }
        }

        private async Task SendFriendRequestEmailAsync(
            CreateFriendRequestDto.RequestLinkGenerator requestLinkGenerator,
            Friendship friendship) =>
            await _mailSender.SendFriendInvitationAsync(
                friendship.RequestedTo.Email,
                requestLinkGenerator(friendship.Id.ToString()));

        private async Task ChangeFriendRequestStatus(int friendRequestId, FriendshipStatus friendshipStatus)
        {
            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == friendRequestId);

            friendship.FriendshipStatus = friendshipStatus;

            await _repository.SaveChangesAsync();
        }
    }
}