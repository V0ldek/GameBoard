using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.Notifications.Old
{
    public interface IOldMailSender
    {
        Task SendEmailConfirmationAsync(string email, string link);
        Task SendEventInvitationAsync(IEnumerable<string> emails, string link);
        Task SendFriendInvitationAsync(string email, string link);
        Task SendPasswordResetAsync(string email, string link);
    }
}