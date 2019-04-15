using System;
using System.Collections.Generic;
using System.Text;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public static class FriendshipExtension
    {
        public static FriendRequestDto ToFriendRequestDto(this Friendship friendship) => new FriendRequestDto(
            friendship.Id.ToString(),
            friendship.RequestedBy.ToUserDto(),
            friendship.RequestedTo.ToUserDto(),
            friendship.FriendshipStatus.ToFriendRequest());
    }
}
