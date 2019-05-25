using System;
using System.Threading;
using System.Threading.Tasks;
using GameBoard.HostedServices.KeepAlive;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace GameBoard.HostedServices
{
    public abstract class CronScheduledService : ScopedHostedService
    {
        private static readonly TimeSpan MaximalIdleTime = TimeSpan.FromMinutes(15);
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
                await base.ExecuteAsync(cancellationToken);
                await WaitUntilDueAsync(cancellationToken);
            }
        }

        protected sealed override async Task ExecuteInScopeAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            await KeepAliveAsync(serviceProvider, cancellationToken);

            if (DateTime.Now >= _nextRun)
            {
                await ExecuteCronJobAsync(serviceProvider, cancellationToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
        }

        private static async Task KeepAliveAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var keepAlive = serviceProvider.GetService<IKeepAlive>();
            await keepAlive.KeepAliveAsync(cancellationToken);
        }

        protected abstract Task ExecuteCronJobAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);

        private async Task WaitUntilDueAsync(CancellationToken cancellationToken)
        {
            var timeToWait = _nextRun - DateTime.Now;
            if (timeToWait > TimeSpan.Zero)
            {
                var idleTime = timeToWait <= MaximalIdleTime ? timeToWait : MaximalIdleTime;
                await Task.Delay(idleTime, cancellationToken);
            }
        }
    }
}