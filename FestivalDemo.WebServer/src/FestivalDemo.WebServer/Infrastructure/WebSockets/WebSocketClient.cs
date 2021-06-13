using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FestivalDemo.WebServer.Infrastructure.WebSockets
{
    /// <summary>
    /// Dispatcher for receiving and sending messages to the client.
    /// </summary>
    public class WebSocketClient
    {
        private const int BufferSize = 4096;

        private readonly ILogger<WebSocketClient> _logger;


        private WebSocket? _webSocket;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        public WebSocketClient(ILogger<WebSocketClient> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Check if the dispatcher has a open web socket connection.
        /// </summary>
        /// <returns>True if there is an open web socket connection</returns>
        public bool IsConnected()
        {
            if (_webSocket == null)
            {
                return false;
            }


            return _webSocket.State == WebSocketState.Open;
        }

        /// <summary>
        /// Register WebSocket and close the old one.
        /// </summary>
        /// <param name="webSocket">WebSocket</param>
        /// <returns>Task</returns>
        public async Task ConnectAsync(WebSocket webSocket)
        {
            if (webSocket is null)
            {
                throw new ArgumentNullException(nameof(webSocket));
            }

            if (_webSocket != null)
            {
                await CloseSocketAsync().ConfigureAwait(true);
            }

            _webSocket = webSocket;
        }

        /// <summary>
        /// Receive packages from the web socket.
        /// </summary>
        /// <returns>Task</returns>
        public async Task<byte[]?> ReceiveAsync()
        {
            if (_webSocket == null)
            {
                return null;
            }

            var buffer = new byte[BufferSize];
            var package = new List<byte>();

            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(true);
            package.AddRange(new ArraySegment<byte>(buffer, 0, result.Count));

            while (!result.EndOfMessage)
            {
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(true);
                package.AddRange(new ArraySegment<byte>(buffer, 0, result.Count));
            }

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                return package.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public async Task SendAsync(byte[] message)
        {
            if (_webSocket != null)
            {
                await _webSocket.SendAsync(message, WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Close socket and send message to cluent.
        /// </summary>
        /// <returns>Task</returns>
        public  async Task CloseSocketAsync()
        {
            try
            {
                if (_webSocket != null)
                {
                    if (_webSocket.State == WebSocketState.Open)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed by server", CancellationToken.None);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Close socket failed with message {ErrorMessage}", exception.Message);
            }

            _webSocket = null;
        }



        
    }
}
