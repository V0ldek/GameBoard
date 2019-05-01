using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Configurations;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GameBoard.LogicLayer.Notifications
{
    public class MailSender : IMailSender
    {
        private MailNotificationsConfiguration MailNotificationsOptions { get; }

        public MailSender(IOptions<MailNotificationsConfiguration> mailNotificationsOptions)
        {
            MailNotificationsOptions = mailNotificationsOptions.Value;
        }

        public Task SendEmailConfirmationAsync(string email, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.EmailConfirmationHtml),
                link,
                "Email Confirmation");
            return SendEmailAsync(email, notification);
        }

        public Task SendEventCancellationAsync(IEnumerable<string> emails, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.EventCancellationHtml),
                link,
                "Event Cancellation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventConfirmationAsync(IEnumerable<string> emails, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.EventConfirmationHtml),
                link,
                "Event Confirmation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventInvitationAsync(IEnumerable<string> emails, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.EventInvitationHtml),
                link,
                "Event Invitation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendFriendInvitationAsync(string email, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.FriendInvitationHtml),
                link,
                "Friend Invitation");
            return SendEmailAsync(email, notification);
        }

        public Task SendPasswordResetAsync(string email, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.PasswordResetHtml),
                link,
                "Password Reset");
            return SendEmailAsync(email, notification);
        }

        private Task SendEmailAsync(IEnumerable<string> emails, Notification notification)
            => Execute(MailNotificationsOptions.SendGridApiKey, emails, notification);

        private Task SendEmailAsync(string email, Notification notification)
        {
            var emails = new List<string>
            {
                email
            };
            return SendEmailAsync(emails, notification);
        }

        private Task Execute(string apiKey, IEnumerable<string> emails, Notification notification)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("GameBoard.contact@gmail.com", "GameBoard");
            var tos = new List<EmailAddress>();
            var subject = notification.Subject;
            var htmlContent = notification.Html;

            emails.ToList().ForEach(email => tos.Add(new EmailAddress(email)));

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, null, htmlContent);

            return client.SendEmailAsync(msg);
        }

        private string GetHtmlPath(string htmlTemplateName)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var htmlPath = Path.Combine(assemblyPath, MailNotificationsOptions.DefaultHtmlPath, htmlTemplateName);
            return htmlPath;
        }
    }
}