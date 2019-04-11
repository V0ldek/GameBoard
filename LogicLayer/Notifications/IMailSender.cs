using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.Notifications
{
    public interface IMailSender
    {
        Task SendEmailConfirmationAsync(IEnumerable<string> emails, string link);
        Task SendEventCancellationAsync(IEnumerable<string> emails, string link);
        Task SendEventConfirmationAsync(IEnumerable<string> emails, string link);
        Task SendEventInvitationAsync(IEnumerable<string> emails, string link);
        Task SendFriendAcceptAsync(IEnumerable<string> emails, string link);
        Task SendFriendInvitationAsync(IEnumerable<string> emails, string link);
        Task SendPasswordResetAsync(IEnumerable<string> emails, string link);
    }
}