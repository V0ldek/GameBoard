using System.Collections.Generic;

namespace GameBoard.Notifications.HtmlTemplateNotifications.Notifications
{
    public class HtmlTemplateNotification : Notification
    {
        protected HtmlTemplateNotification(string subject, IEnumerable<string> recipientsEmails) : base(
            subject,
            recipientsEmails,
            new HtmlTemplateNotificationContentBuilder())
        {
        }

        protected override void BuildContent() => NotificationContentBuilder.AddTitle(Subject);
    }
}