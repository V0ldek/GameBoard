using System.Linq;
using GameBoard.Notifications;
using SendGrid.Helpers.Mail;

namespace GameBoard.LogicLayer.Notifications.NotificationBatch.SendGrid
{
    internal static class NotificationExtensions
    {
        public static SendGridMessage ToSendGridMessage(this INotification notification, string senderEmail, string senderName) =>
            MailHelper.CreateSingleEmailToMultipleRecipients(
                new EmailAddress(senderEmail, senderName),
                notification.RecipientsEmails.Select(e => new EmailAddress(e)).ToList(),
                notification.Subject,
                notification.PlainTextContent,
                notification.HtmlContent);
    }
}