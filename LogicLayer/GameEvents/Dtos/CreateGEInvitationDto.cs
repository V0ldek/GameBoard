using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class CreateGEInvitationDto
    {
        public string SendBy;
        public string SendTo;
        public string InvitedTo;

        public CreateGEInvitationDto([NotNull] string _sendBy, [NotNull] string _sendTo, [NotNull] string _invitedTo)
        {
            SendBy = _sendBy;
            SendTo = _sendTo;
            InvitedTo = _invitedTo;
        }
    }
}
