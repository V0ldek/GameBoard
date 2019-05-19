using System.Collections.Generic;

namespace GameBoard.Notifications.HtmlTemplateNotifications.Notifications
{
    public class TextNotification : HtmlTemplateNotification
    {
        private readonly string[] _paragraphs;

        protected TextNotification(string subject, IEnumerable<string> recipientsEmails, params string[] paragraphs) : base(subject, recipientsEmails)
        {
            _paragraphs = paragraphs;
        }

        protected override void BuildContent()
        {
            base.BuildContent();
            foreach (var paragraph in _paragraphs)
            {
                NotificationContentBuilder.AddText(paragraph);
            }
        }
    }
}
