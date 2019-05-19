using System.Collections.Generic;

namespace GameBoard.Notifications
{
    public interface INotification
    {
        string Subject { get; }

        IEnumerable<string> RecipientsEmails { get; }

        string HtmlContent { get; }

        string PlainTextContent { get; }
    }
}