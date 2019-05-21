namespace GameBoard.LogicLayer.Configurations
{
    public class MailNotificationsConfiguration
    {
        public string SendGridApiKey { get; set; }

        public string FromEmailAddress { get; set; }

        public string FromName { get; set; }
    }
}