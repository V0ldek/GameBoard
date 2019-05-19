using System.Collections.Generic;
using GameBoard.Notifications;
using GameBoard.Notifications.NotificationBatch;

namespace GameBoard.LogicLayer.Notifications
{
    public interface INotificationService
    {
        INotificationBatch CreateNotificationBatch(IEnumerable<INotification> notifications);

        INotificationBatch CreateNotificationBatch(params INotification[] notifications);
    }
}
