using System;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.Infrastructure.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FestivalDemo.WebServer.Middleware
{
    public class WebSocketMiddleware
    {

        private const string WebSocketPath = "/ws";
        private readonly ILogger<WebSocketMiddleware> _logger;
        private readonly RequestDelegate _requestDelegate;
        private readonly WebSocketClient _dispatcher;
        private readonly IServiceBusAdapter _adapter;

        public WebSocketMiddleware(ILogger<WebSocketMiddleware> logger,
            RequestDelegate requestDelegate,
            WebSocketClient dispatcher,
            IServiceBusAdapter adapter
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _requestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!IsValidRequest(context))
            {
                await _requestDelegate.Invoke(context).ConfigureAwait(false);
                return;
            }

            if (!_dispatcher.IsConnected())
            {
                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(true))
                {
                    if (webSocket == null)
                    {
                        return;
                    }

                    try
                    {
                        await _dispatcher.ConnectAsync(webSocket).ConfigureAwait(true);
                        while (_dispatcher.IsConnected())
                        {
                            var bytes = await _dispatcher.ReceiveAsync().ConfigureAwait(true);
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
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Connection failed with message '{0}'.", ex.Message);
                    }

                    try
                    {
                        await _dispatcher.CloseSocketAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unexpected error with message '{0}'." + ex.Message);
                    }
                }
            }
        }

        private static bool IsValidRequest(HttpContext context)
        {
            return context.Request.Path == WebSocketPath
                && context.WebSockets.IsWebSocketRequest;
        }
    }
}
