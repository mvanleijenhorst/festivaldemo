using System;
using System.Threading;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.Infrastructure.WebSockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FestivalDemo.WebServer.BackgroundServices
{
    public class OutgoingBackgroundService : BackgroundService
    {
        private readonly ILogger<OutgoingBackgroundService> _logger;
        private readonly WebSocketClient _client;
        private readonly IServiceBusAdapter _adapter;

        public OutgoingBackgroundService(ILogger<OutgoingBackgroundService> logger,
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
                    if (_adapter.TryGetMessage(ConfigurationConstants.OutgoingQueue, out var message))
                    {
                        if (message != null)
                        {
                            try
                            {
                                var bytes = WebSocketMessageSerializer.Serialize(message);
                                await _client.SendAsync(bytes).ConfigureAwait(true);
                            }
                            catch (NotSupportedException ex)
                            {
                                _logger.LogWarning("Send invalid message '{Exception}'.", ex);
                            }
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken).ConfigureAwait(false);
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
