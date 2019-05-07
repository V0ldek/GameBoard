namespace GameBoard.Models.Groups
{
    public class ManageGroupsViewModel
    {
        public string Username { get; }

        public ManageGroupsViewModel(string username)
        {
            Username = username;
        }
    }
}
