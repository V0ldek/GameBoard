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

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now >= _nextRun)
                {
                    await base.ExecuteAsync(cancellationToken);
                    now = DateTime.Now;
                    _nextRun = _schedule.GetNextOccurrence(now);
                }

                await Task.Delay((int) (_nextRun - now).TotalMilliseconds, cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }
    }
}