using System;

namespace GameBoard.LogicLayer.Notifications
{

    public struct Notification
    {
        public string Subject { get; }
        public string HtmlPath { get;}

        public Notification(string htmlPath, string subject)
        {
            HtmlPath = htmlPath;
            Subject = subject;
        }

    }

}
