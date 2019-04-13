using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

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
            => Execute(Options.MailgunDomain, Options.MailgunApiKey, emails, notification);

        private Task Execute(string domain, string apiKey, IEnumerable<string> emails, Notification notification)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes("api" + ":" + apiKey)));

            var emailsJoined = String.Join(", ", emails);

            Dictionary<string, string> form = new Dictionary<string, string>()
            {
                ["from"] = "GameBoard <mailgun@sandboxb1586a7208744c12afa71d3d21e35535.mailgun.org>",
                ["to"] = emailsJoined,
                ["subject"] = notification.Subject,
                ["html"] = notification.Html
            };

            return client.PostAsync(
                "https://api.mailgun.net/v2/" + domain + "/messages",
                new FormUrlEncodedContent(form));
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