namespace FestivalDemo.WebServer.Infrastructure.WebSockets
{
    public enum WebSocketCommandType
    {
        OpenFestival = 0,
        CloseFestival = 1,
        AddGuest = 2,
        RemoveGuest = 3,
        UpdateGuest = 4,
        BuildingInfo = 5,
    }
}
