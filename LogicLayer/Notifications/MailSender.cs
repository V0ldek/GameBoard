using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GameBoard.LogicLayer.Notifications
{
    public class MailSender : IMailSender
    {
        public MailSender(IOptions<MailNotificationsConfiguration> mailOptionsAccessor)
        {
            MailOptions = mailOptionsAccessor.Value;
        }

        private MailNotificationsConfiguration MailOptions { get; }

        private Task SendEmailAsync(IEnumerable<string> emails, Notification notification)
            => Execute(MailOptions.MailgunDomain, MailOptions.MailgunApiKey, emails, notification);

        private Task SendEmailAsync(string email, Notification notification)
        {
            var emails = new List<string>
            {
                email
            };
            return SendEmailAsync(emails, notification);
        }

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

        public Task SendEmailConfirmationAsync(string email, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.EmailConfirmationHtml), link, "Email Confirmation");
            return SendEmailAsync(email, notification);
        }

        public Task SendEventCancellationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.EventCancellationHtml), link, "Event Cancellation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventConfirmationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.EventConfirmationHtml), link, "Event Confirmation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendEventInvitationAsync(IEnumerable<string> emails, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.EventInvitationHtml), link, "Event Invitation");
            return SendEmailAsync(emails, notification);
        }

        public Task SendFriendAcceptAsync(string email, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.FriendAcceptHtml), link, "Friend Invitation Accepted");
            return SendEmailAsync(email, notification);
        }

        public Task SendFriendInvitationAsync(string email, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.FriendInvitationHtml), link, "Friend Invitation");
            return SendEmailAsync(email, notification);
        }

        public Task SendPasswordResetAsync(string email, string link)
        {
            Notification notification = new Notification(GetHtmlPath(MailOptions.PasswordResetHtml), link, "Password Reset");
            return SendEmailAsync(email, notification);
        }
    }
}