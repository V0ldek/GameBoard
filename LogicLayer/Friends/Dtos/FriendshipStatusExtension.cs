using System;
using GameBoard.DataLayer.Enums;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public static class FriendshipsStatusExtensions
    {
        public static FriendRequestDto.FriendRequestStatus ToDtoStatus(this FriendshipStatus friendshipStatus)
        {
            switch (friendshipStatus)
            {
                case FriendshipStatus.Lasts:
                    return FriendRequestDto.FriendRequestStatus.Accepted;
                case FriendshipStatus.PendingFriendRequest:
                    return FriendRequestDto.FriendRequestStatus.Sent;
                case FriendshipStatus.Rejected:
                    return FriendRequestDto.FriendRequestStatus.Rejected;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"{friendshipStatus} is not included in FriendshipStatus");
            }
        }
    }
}