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
                await RunIfDueAsync(cancellationToken);
                await KeepAliveAsync(cancellationToken);
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

        private async Task KeepAliveAsync(CancellationToken cancellationToken)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var keepAlive = scope.ServiceProvider.GetService<IKeepAlive>();
                await keepAlive.KeepAliveAsync(cancellationToken);
            }
        }

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