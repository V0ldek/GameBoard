using GameBoard.Notifications.HtmlTemplateNotifications.Notifications;

namespace GameBoard.Areas.Identity.Pages.Account.Notifications
{
    internal class PasswordResetNotification : LinkAndTextNotification
    {
        public PasswordResetNotification(string userName, string userEmail, string url) : base(
            "Reset password",
            new[] {userEmail},
            url,
            "Reset password",
            $"Hello {userName}!",
            $"We have received a password reset request for ${userEmail}. Click below to reset your password.",
            "If you have not requested a password reset recently, please ignore this message.")
        {
        }
    }
}