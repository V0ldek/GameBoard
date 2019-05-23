using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GameBoard.Configuration;

namespace GameBoard.HostedServices.KeepAlive
{
    internal class HttpRequestKeepAlive : IKeepAlive
    {
        private readonly HostConfiguration _hostConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpRequestKeepAlive(HostConfiguration hostConfiguration, IHttpClientFactory httpClientFactory)
        {
            _hostConfiguration = hostConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public Task KeepAliveAsync(CancellationToken cancellationToken)
        {
            var url = _hostConfiguration.HostAddress;
            var httpClient = _httpClientFactory.CreateClient();
            return httpClient.GetAsync(url, cancellationToken);
        }
    }
}
