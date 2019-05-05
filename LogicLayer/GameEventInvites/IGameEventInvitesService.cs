using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventInvites.Dtos;

namespace GameBoard.LogicLayer.GameEventInvites
{
    public interface IGameEventInvitesService
    {
        Task SendGameEventInvitationAsync([NotNull] CreateGameEventInvitationDto gameEventInvitationDto);

        Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);

        Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);
    }
}
