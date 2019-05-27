using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

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

        public int Id { get; }

        [NotNull]
        public UserDto UserFrom { get; }

        [NotNull]
        public UserDto UserTo { get; }

        public FriendRequestStatus Status { get; }

        public FriendRequestDto(
            int id,
            [NotNull] UserDto userFrom,
            [NotNull] UserDto userTo,
            FriendRequestStatus status)
        {
            Id = id;
            UserFrom = userFrom;
            UserTo = userTo;
            Status = status;
        }
    }
}