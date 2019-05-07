namespace GameBoard.Models.FriendRequest
{
    public class FriendRequestViewModel
    {
        public int Id { get; }

        public string UserNameFrom { get; }

        public FriendRequestViewModel(int id, string userNameFrom)
        {
            Id = id;
            UserNameFrom = userNameFrom;
        }
    }
}