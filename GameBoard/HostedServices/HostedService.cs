using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace GameBoard.HostedServices
{
    public abstract class HostedService : IHostedService
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _executingTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = ExecuteAsync(_cancellationTokenSource.Token);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}