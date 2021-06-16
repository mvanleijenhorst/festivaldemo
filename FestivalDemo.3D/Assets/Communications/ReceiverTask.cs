using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Communications
{
    internal class ReceiverTask
    {
        private const int BufferSize = 4096;

        private readonly ConcurrentQueue<byte[]> _queue;
        private ClientWebSocket _socket;
        private bool _isRunning;

        internal ReceiverTask()
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

        internal bool TryReceive(out byte[] message)
        {
            return _queue.TryDequeue(out message);
        }

        private async Task Run()
        {
            while (_isRunning)
            {
                while (IsConnected)
                {
                    var buffer = new byte[BufferSize];
                    WebSocketReceiveResult sockerReceiveResult;
                    var bytes = new List<byte>();

                    if (_socket == null)
                    {
                        return;
                    }


                    sockerReceiveResult = await _socket
                        .ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None)
                        .ConfigureAwait(true);

                    switch (sockerReceiveResult.MessageType)
                    {
                        case WebSocketMessageType.Binary:
                            bytes.AddRange(new ArraySegment<byte>(buffer, 0, sockerReceiveResult.Count));

                            while (!sockerReceiveResult.EndOfMessage)
                            {
                                sockerReceiveResult = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(true);
                                bytes.AddRange(new ArraySegment<byte>(buffer, 0, sockerReceiveResult.Count));
                            }

                            if (bytes.Any())
                            {
                                _queue.Enqueue(bytes.ToArray());
                            }
                            break;
                        case WebSocketMessageType.Close:
                            break;
                        case WebSocketMessageType.Text:
                            break;
                    }
                }
            }
        }

        private bool IsConnected => _socket != null && _socket.State == WebSocketState.Open;
    }
}
