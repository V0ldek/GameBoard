using System;
using System.Threading;
using System.Threading.Tasks;
using GameBoard.Configuration;
using GameBoard.LogicLayer.EventReminders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GameBoard.HostedServices.EventReminders
{
    internal class DayBeforeEventReminderCronScheduledService : CronScheduledService
    {
        private const string CronSchedule = "0 20 * * *";

        public DayBeforeEventReminderCronScheduledService(IServiceScopeFactory serviceScopeFactory) : base(
            serviceScopeFactory,
            CronSchedule)
        {
        }

        protected override Task ExecuteInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var hostConfiguration = serviceProvider.GetService<IOptions<HostConfiguration>>().Value;
            var eventReminderService = serviceProvider.GetService<IEventReminderService>();
            var tomorrow = DateTime.Today.AddDays(1);
            // This link generation is less than ideal, but I have no other ideas at the moment.
            return eventReminderService.SendRemindersIfDateBetweenAsync(
                tomorrow,
                tomorrow.AddDays(1),
                id => hostConfiguration.HostAddress + $"/GameEvent/GameEvent/{id}");
        }
    }
}