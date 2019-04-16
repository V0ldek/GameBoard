using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GameBoard.LogicLayer.Notifications
{
    public class MailSender : IMailSender
    {
        public MailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IOptions<MailNotificationsConfiguration> mailOptionsAccessor)
        {
            Options = optionsAccessor.Value;
            MailOptions = mailOptionsAccessor.Value;
        }

        private AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        private MailNotificationsConfiguration MailOptions { get; }

        private Task SendEmailAsync(IEnumerable<string> emails, Notification notification)
            => Execute(Options.MailgunApiKey, emails, notification);

        private Task Execute(string apiKey, IEnumerable<string> emails, Notification notification)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("GameBoard.contact@gmail.com", "The Gameboard Team");
            var tos = new List<EmailAddress>();
            var subject = notification.Subject;
            var htmlContent = notification.Html;

            foreach (string email in emails)
            {
                tos.Add(new EmailAddress(email));
            }

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, null, htmlContent);

            return client.SendEmailAsync(msg);
        }

        private string GetHtmlPath(string htmlTemplateName)
        {
            string htmlPath = MailOptions.DefaultHtmlPath;
            htmlPath = Path.Combine(htmlPath, htmlTemplateName);
            return htmlPath;
        }

        public Task SendEmailConfirmationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("email-confirmation.html"), link, "Email Confirmation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventCancellationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("event-cancellation.html"), link, "Event Cancellation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventConfirmationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("event-confirmation.html"), link, "Event Confirmation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventInvitationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("event-invitation.html"), link, "Event Invitation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendFriendAcceptAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("friend-accept.html"), link, "Friend Invitation Accepted");
            return SendEmailAsync(emails, notification);
        }

        public Task SendFriendInvitationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("friend-invitation.html"), link, "Friend Invitation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendPasswordResetAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath("password-reset.html"), link, "Password Reset");
            return SendEmailAsync(emails, notification);
        }
    }
}