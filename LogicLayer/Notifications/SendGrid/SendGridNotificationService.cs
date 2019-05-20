using System.Collections.Generic;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Notifications.NotificationBatch.SendGrid;
using GameBoard.Notifications;
using GameBoard.Notifications.NotificationBatch;
using Microsoft.Extensions.Options;

namespace GameBoard.LogicLayer.Notifications.SendGrid
{
    internal sealed class SendGridNotificationService : INotificationService
    {
        private readonly MailNotificationsConfiguration _configuration;

        public SendGridNotificationService(IOptions<MailNotificationsConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public INotificationBatch CreateNotificationBatch(params INotification[] notifications) =>
            CreateNotificationBatch(notifications as IEnumerable<INotification>);

        public INotificationBatch CreateNotificationBatch(IEnumerable<INotification> notifications) =>
            new SendGridNotificationBatch(_configuration, notifications);
    }
}