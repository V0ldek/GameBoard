using System.Collections.Generic;

namespace GameBoard.Notifications.HtmlTemplateNotifications.Notifications
{
    public class LinkAndTextNotification : TextNotification
    {
        private readonly string _linkHref;

        private readonly string _linkText;

        protected LinkAndTextNotification(
            string subject,
            IEnumerable<string> recipientsEmails,
            string linkHref,
            string linkText,
            params string[] paragraphs) : base(subject, recipientsEmails, paragraphs)
        {
            _linkHref = linkHref;
            _linkText = linkText;
        }

        protected override void BuildContent()
        {
            base.BuildContent();
            NotificationContentBuilder.AddLink(_linkHref, _linkText);
            PlainTextContentBuilder.AppendLine(_linkText).AppendLine(_linkHref);
        }
    }
}