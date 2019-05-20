using GameBoard.Notifications.HtmlTemplateNotifications.Notifications;

namespace GameBoard.LogicLayer.Friends.Notifications
{
    internal sealed class FriendRequestNotification : LinkAndTextNotification
    {
        public FriendRequestNotification(
            string senderUserName,
            string recipientUserName,
            string recipientEmail,
            string friendRequestUrl) : base(
            "Friend request",
            new[] {recipientEmail},
            friendRequestUrl,
            "See friend request",
            $"Hello, {recipientUserName}!",
            $"You have received a friend request from {senderUserName}. Click on the link below to review it.")
        {
        }
    }
}