using System;
using GameBoard.Notifications.HtmlTemplateNotifications.Notifications;

namespace GameBoard.LogicLayer.GameEventReminders.Notifications
{
    internal sealed class EventReminderNotification : LinkAndTextNotification
    {
        public EventReminderNotification(
            string gameEventName,
            string recipientName,
            string recipientEmail,
            DateTime date,
            string url) : base(
            "Game event reminder",
            new[] {recipientEmail},
            url,
            "See event details",
            $"Hello, {recipientName}!",
            $"The event {gameEventName} in which you are participating will begin soon ({date:dd.MM.yyyy HH:mm}).")
        {
        }
    }
}