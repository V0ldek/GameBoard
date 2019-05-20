using System;
using System.Collections.Generic;
using System.Text;
using GameBoard.Notifications.NotificationContentBuilder;

namespace GameBoard.Notifications
{
    public abstract class Notification : INotification
    {
        private readonly Lazy<string> _htmlContentLazy;

        protected readonly INotificationContentBuilder NotificationContentBuilder;

        protected readonly StringBuilder PlainTextContentBuilder = new StringBuilder();

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

        public string Subject { get; }

        public IEnumerable<string> RecipientsEmails { get; }

        public string HtmlContent => _htmlContentLazy.Value;

        public string PlainTextContent => PlainTextContentBuilder.ToString();

        protected abstract void BuildContent();

        private string RenderContent()
        {
            BuildContent();
            return NotificationContentBuilder.Build();
        }
    }
}