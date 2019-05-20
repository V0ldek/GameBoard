using GameBoard.Notifications.HtmlTemplateNotifications.Notifications;

namespace GameBoard.LogicLayer.GameEventParticipations.Notifications
{
    internal sealed class GameEventInvitationNotification : LinkAndTextNotification
    {
        public GameEventInvitationNotification(
            string gameEventName,
            string senderUserName,
            string recipientUserName,
            string recipientEmail,
            string gameEventUrl) : base(
            "Game event invitation",
            new []{recipientEmail},
            gameEventUrl,
            "See event details",
            $"Hello {recipientUserName}!",
            $"You have been invited by {senderUserName} to the {gameEventName} event.",
            $"Click on the link below to see the event details.")
        {
        }
    }
}