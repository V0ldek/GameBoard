namespace GameBoard.Notifications.NotificationContentBuilder
{
    public interface INotificationContentBuilder
    {
        INotificationContentBuilder AddTitle(string title);

        INotificationContentBuilder AddText(string text);

        INotificationContentBuilder AddLink(string href, string text);

        string Build();
    }
}
