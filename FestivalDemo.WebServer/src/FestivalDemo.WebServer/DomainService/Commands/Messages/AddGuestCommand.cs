using FestivalDemo.WebServer.Common.Commands;

namespace FestivalDemo.WebServer.DomainServices.Commands.Messages
{
    public record AddGuestCommand(int GuestId, bool IsFollower);
}
