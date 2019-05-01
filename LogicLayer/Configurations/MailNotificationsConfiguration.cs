namespace GameBoard.LogicLayer.Configurations
{
    public class MailNotificationsConfiguration
    {
        public string SendGridApiKey { get; set; }
        public string DefaultHtmlPath { get; set; }
        public string EmailConfirmationHtml { get; set; }
        public string EventCancellationHtml { get; set; }
        public string EventInvitationHtml { get; set; }
        public string FriendInvitationHtml { get; set; }
        public string PasswordResetHtml { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromEmailName { get; set; }
    }
}