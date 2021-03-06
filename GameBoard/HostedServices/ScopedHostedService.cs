﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GameBoard.HostedServices
{
    public abstract class ScopedHostedService : HostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected ScopedHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ExecuteInScopeAsync(scope.ServiceProvider, cancellationToken);
            }
        }

        protected abstract Task ExecuteInScopeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}