using GameBoard.LogicLayer.Groups.Dtos;

namespace GameBoard.Models.GameEventInviteForm
{
    public class GameEventInviteFormViewModel
    {
        public int GameEventId { get; }

        public string GameEventName { get; }

        public string UserName { get; }

        public GroupDto Group { get; }

        public GameEventInviteFormViewModel(int gameEventId, string gameEventName, string userName)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            UserName = userName;
        }

        public GameEventInviteFormViewModel(int gameEventId, string gameEventName, GroupDto group)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            Group = group;
        }
    }
}