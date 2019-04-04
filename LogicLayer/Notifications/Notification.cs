using System;

namespace GameBoard.LogicLayer.Notifications
{

    public struct Notification
    {
        public string HtmlPath { get;}
        public string Subject { get; }

        public Notification(string htmlPath, string subject)
        {
            HtmlPath = htmlPath;
            Subject = subject;
        }

    }

}
