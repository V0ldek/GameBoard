using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public sealed class SendFriendRequestDto
    {
        [NotNull]
        public delegate string RequestLinkGenerator([NotNull] string id);

        private readonly RequestLinkGenerator _requestLinkGenerator;

        [NotNull]
        public string UserNameFrom { get; }

        [NotNull]
        public string UserNameTo { get; }

        public SendFriendRequestDto(
            [NotNull] string userNameFrom,
            [NotNull] string userNameTo,
            [NotNull] RequestLinkGenerator requestLinkGenerator)
        {
            UserNameFrom = userNameFrom;
            UserNameTo = userNameTo;
            _requestLinkGenerator = requestLinkGenerator;
        }

        [NotNull]
        public string GenerateRequestLink([NotNull] string id) => _requestLinkGenerator(id);
    }
}