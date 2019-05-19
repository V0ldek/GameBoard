using System;
using System.Collections.Generic;
using GameBoard.Notifications.NotificationContentBuilder;

namespace GameBoard.Notifications
{
    public abstract class Notification : INotification
    {
        public string Subject { get; }

        public IEnumerable<string> RecipientsEmails { get; }

        public string HtmlContent => _htmlContentLazy.Value;

        public string PlainTextContent { get; } = "";

        protected readonly INotificationContentBuilder NotificationContentBuilder;

        private readonly Lazy<string> _htmlContentLazy;

        protected Notification(
            string subject,
            IEnumerable<string> recipientsEmails,
            INotificationContentBuilder notificationContentBuilder)
        {
            Subject = subject;
            RecipientsEmails = recipientsEmails;
            NotificationContentBuilder = notificationContentBuilder;
            _htmlContentLazy = new Lazy<string>(RenderContent);
        }

        protected abstract void BuildContent();

        private string RenderContent()
        {
            BuildContent();
            return NotificationContentBuilder.Build();
        }
    }
}