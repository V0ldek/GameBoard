using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Notifications.Exceptions;
using GameBoard.Notifications;
using GameBoard.Notifications.NotificationBatch;
using SendGrid;

namespace GameBoard.LogicLayer.Notifications.NotificationBatch.SendGrid
{
    internal class SendGridNotificationBatch : INotificationBatch
    {
        private readonly MailNotificationsConfiguration _configuration;
        private readonly List<INotification> _notifications = new List<INotification>();

        public SendGridNotificationBatch(
            MailNotificationsConfiguration configuration,
            IEnumerable<INotification> notifications)
        {
            _configuration = configuration;
            _notifications.AddRange(notifications);
        }

        public void Add(INotification notification) => _notifications.Add(notification);

        public Task SendAsync()
        {
            var sendGridClient = new SendGridClient(_configuration.SendGridApiKey);
            var sendTasks = _notifications
                .Select(n => n.ToSendGridMessage(_configuration.FromEmailAddress, _configuration.FromName))
                .Select(m => sendGridClient.SendEmailAsync(m))
                .Select(AssertResponseTaskSuccessAsync);
            return Task.WhenAll(sendTasks);
        }

        private static async Task AssertResponseTaskSuccessAsync(Task<Response> respondingTask)
        {
            var response = await respondingTask;
            if (!IsSuccessStatusCode(response.StatusCode))
            {
                throw new SendGridNotificationSendException(
                    $"SendGrid API responded with code {response.StatusCode}. " +
                    $"Response body:\n${response.Body}");
            }
        }

        private static bool IsSuccessStatusCode(HttpStatusCode statusCode) => (int) statusCode % 100 == 2;
    }
}