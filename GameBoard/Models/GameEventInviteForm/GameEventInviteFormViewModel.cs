using GameBoard.Models.Groups;

namespace GameBoard.Models.GameEventInviteForm
{
    public class GameEventInviteFormViewModel
    {
        public int GameEventId { get; }

        public string GameEventName { get; }

        public string UserName { get; }

        public GroupViewModel Group { get; }

        public GameEventInviteFormViewModel(int gameEventId, string gameEventName, string userName)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            UserName = userName;
        }

        public GameEventInviteFormViewModel(int gameEventId, string gameEventName, GroupViewModel group)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            Group = group;
        }
    }
}