using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventInvites.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEventInvites
{
    public interface IGameEventInvitesService
    {
        Task SendGameEventInvitationAsync([NotNull] CreateGameEventInvitationDto gameEventInvitationDto);

        Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);

        Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);
    }
}