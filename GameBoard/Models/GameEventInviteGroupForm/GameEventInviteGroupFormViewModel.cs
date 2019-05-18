using GameBoard.LogicLayer.Groups.Dtos;

namespace GameBoard.Models.GameEventInviteGroupForm
{
    public class GameEventInviteGroupFormViewModel
    {
        public int GameEventId { get; }
        public string GameEventName { get; }

        public int GroupId { get; }
        public string GroupName { get; }

        public GameEventInviteGroupFormViewModel(int gameEventId, string gameEventName, int groupId, string groupName)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}