using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Communications
{
    public class WebSocketClient : IDisposable
    {
        private static WebSocketClient _instance;

        public static WebSocketClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WebSocketClient();
            }

            return _instance;
        }

        private readonly ClientWebSocket _socket;
        private readonly ReceiverTask _receiverTask;
        private readonly SenderTask _senderTask;

        private WebSocketClient()
        {
            _socket = new ClientWebSocket();
            _receiverTask = new ReceiverTask();
            _senderTask = new SenderTask();
        }

        public async Task Connect(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            try
            {
                Debug.Log("Connecting to: " + uri);
                await _socket.ConnectAsync(uri, CancellationToken.None).ConfigureAwait(true);
                while (_socket.State == WebSocketState.Connecting)
                {
                    Debug.Log("Waiting to connect...");
                    Task.Delay(50).Wait();
                }

                Debug.Log("Connect status: " + _socket.State);
                _receiverTask.Start(_socket);
                _senderTask.Start(_socket);
            }
            catch (Exception ex)
            { 
                Debug.LogException(ex);
            }
        }

        public void Send(byte[] message)
        {
            _senderTask.Send(message);
        }

        public bool TryReceive(out byte[] message)
        {
            return _receiverTask.TryReceive(out message);
        }

        public void Dispose()
        {
            _senderTask?.Stop();
            _receiverTask?.Stop();
            _socket?.Dispose();
        }

        



    }
}
