namespace FestivalDemo.WebServer.DomainServices.Commands.Messages
{
    public record BuildingInfoCommand(int BuildingId, int BuildingType, float Longitude, float Latitude);
}
