using System;
using System.Collections.Generic;
using System.Text;
using GameBoard.DataLayer.Enums;

// I know that it is not the best namespace to put this in, but it did not work in other ones.

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public static class FriendshipsStatusExtensions
    {
        public static FriendRequestDto.FriendRequestStatus ToFriendRequest(this FriendshipStatus f)
        {
            switch (f)
            {
                case FriendshipStatus.Lasts:
                    return FriendRequestDto.FriendRequestStatus.Accepted;
                case FriendshipStatus.PendingFriendRequest:
                    return FriendRequestDto.FriendRequestStatus.Sent;
                case FriendshipStatus.Rejected:
                    return FriendRequestDto.FriendRequestStatus.Rejected;
                default:
                    throw new InvalidOperationException("This status is not included in FriendshipStatus");
            }
        }
    }
}
