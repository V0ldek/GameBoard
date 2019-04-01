using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public sealed class CreateFriendRequestDto
    {
        [NotNull]
        public string UserNameFrom { get; }

        [NotNull]
        public string UserNameTo { get; }

        public CreateFriendRequestDto([NotNull] string userNameFrom, [NotNull] string userNameTo)
        {
            UserNameFrom = userNameFrom;
            UserNameTo = userNameTo;
        }
    }
}