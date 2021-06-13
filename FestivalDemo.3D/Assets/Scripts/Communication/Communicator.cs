using Communications;
using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Forefront class for the server communication.
/// </summary>
public class Communicator : MonoBehaviour
{
    // Server IP address
    [SerializeField]
    private string _hostIP;
    // Server port
    [SerializeField]
    private int _port = 5001;
    // Flag to use localhost
    [SerializeField]
    private bool _useLocalhost = true;
    // Address used in code
    private string _host => _useLocalhost ? "localhost" : _hostIP;


    /// <summary>
    /// Unity method called on initialization.
    /// </summary>
    public async Task Start()
    {
        var client = WebSocketClient.GetInstance();
        var serverUrl = "wss://" + _host + ":" + _port + "/ws";

        await client.Connect(new Uri(serverUrl)).ConfigureAwait(true);
    }


    /// <summary>
    /// Unity method called every frame.
    /// </summary>
    public void Update()
    {
        var client = WebSocketClient.GetInstance();
        while (client.TryReceive(out var message))
        {
            HandleMessage(message);
        }
    }

    /// <summary>
    /// Method responsible for handling server messages.
    /// </summary>
    /// <param name="msg">Message.</param>
    private void HandleMessage(byte[] message)
    {
        Debug.Log("Server: " + message);
    }

    /// <summary>
    /// Method which sends data through websocket.
    /// </summary>
    /// <param name="message">Message in bytes</param>
    public void SendRequest(byte[] message)
    {
        var client = WebSocketClient.GetInstance();
        client.Send(message);
    }
}

