using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public sealed class FriendRequestDto
    {
        public string Id { get; }
        public UserDto UserFrom { get; }
        public UserDto UserTo { get; }

        public FriendRequestDto(string id, UserDto userFrom, UserDto userTo)
        {
            Id = id;
            UserFrom = userFrom;
            UserTo = userTo;
        }
    }
}
