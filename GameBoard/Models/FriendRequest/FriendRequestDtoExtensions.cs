using GameBoard.LogicLayer.Friends.Dtos;

namespace GameBoard.Models.FriendRequest
{
    public static class FriendRequestDtoExtensions
    {
        public static FriendRequestViewModel ToViewModel(this FriendRequestDto friendRequestDto) =>
            new FriendRequestViewModel(friendRequestDto.Id, friendRequestDto.UserFrom.UserName);
    }
}