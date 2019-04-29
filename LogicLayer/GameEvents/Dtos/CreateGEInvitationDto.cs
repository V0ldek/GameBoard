using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class CreateGEInvitationDto
    {
        [NotNull]
        public string SendBy { get; }

        [NotNull]
        public string SendTo { get; }

        public int InvitedTo { get; }

        public CreateGEInvitationDto([NotNull] string sendBy, [NotNull] string sendTo, int invitedTo)
        {
            SendBy = sendBy;
            SendTo = sendTo;
            InvitedTo = invitedTo;
        }
    }
}
