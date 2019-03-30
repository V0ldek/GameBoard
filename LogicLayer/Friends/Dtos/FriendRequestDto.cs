using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public sealed class FriendRequestDto
    {
        public enum FriendRequestStatus
        {
            Sent,
            Accepted,
            Rejected
        }

        public string Id { get; }
        public UserDto UserFrom { get; }
        public UserDto UserTo { get; }
        public FriendRequestStatus Status { get; }

        public FriendRequestDto(string id, UserDto userFrom, UserDto userTo, FriendRequestStatus status)
        {
            Id = id;
            UserFrom = userFrom;
            UserTo = userTo;
            Status = status;
        }
    }
}