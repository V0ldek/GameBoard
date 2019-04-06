using System;
using System.Collections.Generic;
using System.Text;
using GameBoard.DataLayer.Enums;

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
                    throw new InvalidOperationException("FriendshipStatus includes more status");
            }
        }
    }
}
