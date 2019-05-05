using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.GameEventInvitations
{
    public interface IGameEventInvitationsService
    {
        Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userName);

        Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);

        Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);
    }
}
