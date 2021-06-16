using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Communications
{
    internal class SenderTask
    {
        private ClientWebSocket _socket;
        private bool _isRunning;

        private readonly ConcurrentQueue<byte[]> _queue;

        internal SenderTask()
        {
            _isRunning = false;
            _queue = new ConcurrentQueue<byte[]>();
        }

        internal void Start(ClientWebSocket socket)
        {
            if (socket == null)
            {
                throw new NullReferenceException(nameof(socket));
            }

            _socket = socket;
            _isRunning = true;

            Task.Run(() => Run());
        }

        internal void Stop()
        {
            _isRunning = false;
        }
        internal void Send(byte[] message)
        {
            if (_queue.Count() < 2000)
            {
                _queue.Enqueue(message);
            }
        }

        private async Task Run()
        {
            while (_isRunning)
            {
                while (IsConnected && _socket != null)
                {
                    if (_queue.TryDequeue(out var bytes))
                    {
                        if (bytes is null || !bytes.Any())
                        {
                            return;
                        }

                        var byteArraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

                        await _socket
                            .SendAsync(byteArraySegment, WebSocketMessageType.Binary, true, CancellationToken.None)
                            .ConfigureAwait(true);
                    }
                }
            }
        }

        private bool IsConnected => _socket != null && _socket.State == WebSocketState.Open;

    }
}
