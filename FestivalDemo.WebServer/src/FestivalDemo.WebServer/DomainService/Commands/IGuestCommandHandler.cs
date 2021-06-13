using FestivalDemo.WebServer.Common.Commands;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.DomainService.Commands
{
    public interface IGuestCommandHandler :
        ICommandHandler<UpdateGuestCommand>
    {
    }
}
