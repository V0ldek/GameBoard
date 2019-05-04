namespace GameBoard.Models.GameEventInviteForm
{
    public class GameEventInviteFormViewModel
    {
        public int GameEventId { get; }

        public string UserName { get; }

        public GameEventInviteFormViewModel(int gameEventId, string userName)
        {
            GameEventId = gameEventId;
            UserName = userName;
        }
    }
}