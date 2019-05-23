using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEventParticipations.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEventParticipations
{
    public interface IGameEventParticipationService
    {
        Task CreateGameEventParticipationAsync([NotNull] SendGameEventInvitationDto gameEventInvitationDto);

        Task CreateGameEventParticipationAsync(
            int gameEventId,
            [NotNull] SendGameEventInvitationDto.GameEventLinkGenerator gameEventLinkGenerator,
            [NotNull] IEnumerable<string> users);

        Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);

        Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName);

        Task ExitGameEventAsync(int gameEventId, [NotNull] string userName);

        Task RemoveFromGameEventAsync(int gameEventId, [NotNull] string userName);
    }
}