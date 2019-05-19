using GameBoard.Notifications;
using GameBoard.Notifications.NotificationBatch;

namespace GameBoard.LogicLayer.Notifications
{
    public interface INotificationService
    {
        INotificationBatch CreateNotificationBatch(params INotification[] notifications);
    }
}
