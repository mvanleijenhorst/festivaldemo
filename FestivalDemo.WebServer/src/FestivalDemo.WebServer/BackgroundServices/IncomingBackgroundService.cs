using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.Infrastructure.WebSockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FestivalDemo.WebServer.BackgroundServices
{
    public class IncomingBackgroundService : BackgroundService
    {
        private readonly ILogger<IncomingBackgroundService> _logger;
        private readonly WebSocketClient _client;
        private readonly IServiceBusAdapter _adapter;

        public IncomingBackgroundService(ILogger<IncomingBackgroundService> logger,
            WebSocketClient client,
            IServiceBusAdapter adapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_client.IsConnected())
                {
                    var bytes = await _client.ReceiveAsync().ConfigureAwait(true);
                    try
                    {
                        if (bytes != null && bytes.Any())
                        {
                            var message = WebSocketMessageSerializer.Deserialize(bytes);
                            _adapter.AddMessage(ConfigurationConstants.IncomingQueue, message);
                        }
                    }
                    catch (NotSupportedException ex)
                    {
                        _logger.LogWarning("Retreive invalid message '{Exception}'.", ex);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
