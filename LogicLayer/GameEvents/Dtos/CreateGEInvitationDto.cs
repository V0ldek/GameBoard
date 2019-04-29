using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class CreateGEInvitationDto
    {
        [NotNull]
        public string SendByUserName { get; }

        [NotNull]
        public string SendToUserName { get; }

        public int InvitedTo { get; }

        public CreateGEInvitationDto([NotNull] string sendByUserName, [NotNull] string sendToUserName, int invitedTo)
        {
            SendByUserName = sendByUserName;
            SendToUserName = sendToUserName;
            InvitedTo = invitedTo;
        }
    }
}
