using System.Threading;
using System.Threading.Tasks;

namespace GameBoard.HostedServices.KeepAlive
{
    internal interface IKeepAlive
    {
        Task KeepAliveAsync(CancellationToken cancellationToken);
    }
}