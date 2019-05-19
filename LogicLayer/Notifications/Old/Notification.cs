using System.IO;

namespace GameBoard.LogicLayer.Notifications.Old
{
    public struct Notification
    {
        public string Html { get; }
        public string Subject { get; }

        public Notification(string htmlPath, string link, string subject)
        {
            Html = File.ReadAllText(htmlPath).Replace("#@redirectLink@#", link);
            Subject = subject;
        }
    }
}