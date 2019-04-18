namespace GameBoard.Models.FriendRequest
{
    public class FriendRequestViewModel
    {
        public string Id { get; }

        public string UserNameFrom { get; }

        public FriendRequestViewModel(string id, string userNameFrom)
        {
            Id = id;
            UserNameFrom = userNameFrom;
        }
    }
}