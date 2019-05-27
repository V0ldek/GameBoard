using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public static class FriendshipExtensions
    {
        public static FriendRequestDto ToDto(this Friendship friendship) => new FriendRequestDto(
            friendship.Id,
            friendship.RequestedBy.ToDto(),
            friendship.RequestedTo.ToDto(),
            friendship.FriendshipStatus.ToDtoStatus());
    }
}