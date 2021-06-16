using Common;
using Communications;
using Communications.Messages;
using Scripts.Players;
using Serializations;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Forefront class for the server communication.
/// </summary>
public class Communicator : MonoBehaviour
{
    private const string Prefab = "Ellen";

    [SerializeField]
    private string _hostIP;
    [SerializeField]
    private int _port = 5001;
    [SerializeField]
    private bool _useLocalhost = true;
    private string _host => _useLocalhost ? "localhost" : _hostIP;
    
    [SerializeField]
    private GameObject _parentGameObject;

    public async Task Start()
    {
        string serverUrl;
        try
        {
            serverUrl = File.ReadAllText("server.info");
        }
        catch 
        {
            serverUrl = "wss://" + _host + ":" + _port + "/ws";
        }

        var client = WebSocketClient.GetInstance();
        await client.Connect(new Uri(serverUrl)).ConfigureAwait(true);

        SendBuildings("Dixie", BuildingType.Dixie);
        SendBuildings("Podium", BuildingType.Podium);
        SendBuildings("EHBO", BuildingType.Ehbo);
        SendBuildings("Stand", BuildingType.Stand);
    }

    public void Update()
    {
        var client = WebSocketClient.GetInstance();
        while (client.TryReceive(out var message))
        {
            HandleMessage(message);
        }
    }

    public void SendRequest(byte[] message)
    {
        var client = WebSocketClient.GetInstance();
        client.Send(message);
    }

    private void HandleMessage(byte[] bytes)
    {
        var obj = WebSocketMessageSerializer.Deserialize(bytes);
        switch (obj)
        {
            case AddGuestCommand cmd:
                CreateGuest(cmd);
                break;
            case RemoveGuestCommand cmd:
                RemoveGuest(cmd);
                break;
        }
    }

    private void SendBuildings(string tag, BuildingType type)
    {
        var client = WebSocketClient.GetInstance();        
        GameObject[] list;

        try
        {
            list = GameObject.FindGameObjectsWithTag(tag)
                ?.OrderBy(i => i.name)
                ?.ToArray();

            if (list != null && list.Any())
            {
                for (int index = 0; index < list.Length; index++)
                {
                    var buildingId = (int)type * 100 + index;
                    var longitude = gameObject.transform.position.x;
                    var latitude = gameObject.transform.position.z;
                    var obj = new BuildingInfoCommand(index, (int)type, longitude, latitude);
                    var bytes = WebSocketMessageSerializer.Serialize(obj);
                    client.Send(bytes);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    private void RemoveGuest(RemoveGuestCommand command)
    {
        var gameObject = GameObject.Find($"guest_{command.GuestId}");
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void CreateGuest(AddGuestCommand command)
    {
        try
        {
            var prefab = Resources.Load<GameObject>(Prefab);
            
            var position = new Vector3(-100, 0, 49);
            var rotation = Quaternion.Euler(0, 180, 0);

            var initiateGameObject = Instantiate(prefab, position, rotation, _parentGameObject.transform);

            initiateGameObject.name = $"guest_{command.GuestId}";
            var componentList = initiateGameObject.GetComponents<PositionAgent>();

            foreach (var component in componentList)
            {
                component.Communicator = this;

                component.GuestId = command.GuestId;
                component.IsFollower = command.IsFollower;
            }
            Debug.Log($"Guest(Id: {command.GuestId}, IsFollower: {command.IsFollower}) added");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}

