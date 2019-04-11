using System;

namespace GameBoard.LogicLayer.Notifications
{

    public struct Recipient
    {
        public string EmailAddress { get; }
        public string NotificationLink { get; }

        public Recipient(string emailAddress, string notificationLink)
        {
            EmailAddress = emailAddress;
            NotificationLink = notificationLink;
        }
    }

}
