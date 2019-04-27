namespace GameBoard.LogicLayer.Notifications
{
    public class MailNotificationsConfiguration
    {
        public string SendGridApiKey { get; set; }
        public string DefaultHtmlPath { get; set; }
        public string EmailConfirmationHtml { get; set; }
        public string EventCancellationHtml { get; set; }
        public string EventConfirmationHtml { get; set; }
        public string EventInvitationHtml { get; set; }
        public string FriendAcceptHtml { get; set; }
        public string FriendInvitationHtml { get; set; }
        public string PasswordResetHtml { get; set; }
    }
}