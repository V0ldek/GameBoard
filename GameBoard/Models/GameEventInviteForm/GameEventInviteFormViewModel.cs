namespace GameBoard.Models.GameEventInviteForm
{
    public class GameEventInviteFormViewModel
    {
        public int GameEventId { get; }

        public string GameEventName { get; }

        public string UserName { get; }

        public GameEventInviteFormViewModel(int gameEventId, string gameEventName, string userName)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            UserName = userName;
        }
    }
}