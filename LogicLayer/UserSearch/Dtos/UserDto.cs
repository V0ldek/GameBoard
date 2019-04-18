using JetBrains.Annotations;

namespace GameBoard.LogicLayer.UserSearch.Dtos
{
    public sealed class UserDto
    {
        [NotNull]
        public string Id { get; }

        [NotNull]
        public string UserName { get; }

        [NotNull]
        public string Email { get; }

        public UserDto([NotNull] string id, [NotNull] string userName, [NotNull] string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }
    }
}