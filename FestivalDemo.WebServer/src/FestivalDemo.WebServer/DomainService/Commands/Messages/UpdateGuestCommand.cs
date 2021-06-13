using FestivalDemo.WebServer.Common.Commands;

namespace FestivalDemo.WebServer.DomainServices.Commands.Messages
{
    public record UpdateGuestCommand(int GuestId, float Longitude, float Latitude);
}
