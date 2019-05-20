using System.Threading.Tasks;

namespace GameBoard.Notifications.NotificationBatch
{
    public interface INotificationBatch
    {
        void Add(INotification notification);

        Task SendAsync();
    }
}