using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.Notifications
{
    public interface IMailSender
    {
        Task SendEmailConfirmationAsync(string email, string link);
        Task SendEventCancellationAsync(IEnumerable<string> emails, string link);
        Task SendEventConfirmationAsync(IEnumerable<string> emails, string link);
        Task SendEventInvitationAsync(IEnumerable<string> emails, string link);
        Task SendFriendAcceptAsync(string email, string link);
        Task SendFriendInvitationAsync(string email, string link);
        Task SendPasswordResetAsync(string email, string link);
    }
}