using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GameBoard.HostedServices
{
    public abstract class ScopedHostedService : HostedService
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        protected ScopedHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                await ExecuteInScope(scope.ServiceProvider, cancellationToken);
            }
        }

        protected abstract Task ExecuteInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}