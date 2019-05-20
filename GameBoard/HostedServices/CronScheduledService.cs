using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace GameBoard.HostedServices
{
    public abstract class CronScheduledService : ScopedHostedService
    {
        private readonly CrontabSchedule _schedule;
        private DateTime _nextRun;

        protected CronScheduledService(
            IServiceScopeFactory serviceScopeFactory,
            string cronSchedule) : base(serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(cronSchedule);
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected sealed override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RunIfDueAsync(cancellationToken);
                await WaitUntilDueAsync(cancellationToken);
            }
        }

        private async Task RunIfDueAsync(CancellationToken cancellationToken)
        {
            if (DateTime.Now >= _nextRun)
            {
                await base.ExecuteAsync(cancellationToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
        }

        private async Task WaitUntilDueAsync(CancellationToken cancellationToken)
        {
            var timeToWait = _nextRun - DateTime.Now;
            if (timeToWait > TimeSpan.Zero)
            {
                await Task.Delay(timeToWait, cancellationToken);
            }
        }
    }
}