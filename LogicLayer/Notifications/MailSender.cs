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
            return SendNotificationAsync(email, notification);
        }

        public Task SendEventCancellationAsync(IEnumerable<string> emails, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.EventCancellationHtml),
                link,
                "Event Cancellation");
            return SendNotificationAsync(emails, notification);
        }

        public Task SendEventInvitationAsync(IEnumerable<string> emails, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.EventInvitationHtml),
                link,
                "Event Invitation");
            return SendNotificationAsync(emails, notification);
        }

        public Task SendFriendInvitationAsync(string email, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.FriendInvitationHtml),
                link,
                "Friend Invitation");
            return SendNotificationAsync(email, notification);
        }

        public Task SendPasswordResetAsync(string email, string link)
        {
            var notification = new Notification(
                GetHtmlPath(MailNotificationsOptions.PasswordResetHtml),
                link,
                "Password Reset");
            return SendNotificationAsync(email, notification);
        }

        private Task SendNotificationAsync(string email, Notification notification)
        {
            var emails = new List<string>
            {
                email
            };
            return SendNotificationAsync(emails, notification);
        }

        private Task SendNotificationAsync(IEnumerable<string> emails, Notification notification)
        {
            var client = new SendGridClient(MailNotificationsOptions.SendGridApiKey);
            var from = new EmailAddress(
                MailNotificationsOptions.FromEmailAddress,
                MailNotificationsOptions.FromName);
            var tos = emails.Select(e => new EmailAddress(e)).ToList();
            var subject = notification.Subject;
            var htmlContent = notification.Html;

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
                from,
                tos,
                subject,
                "Unfortunately, we doesn't provide our emails in plain text.",
                htmlContent);

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