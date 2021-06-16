using System;
using FestivalDemo.WebServer.Domain.Services;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.DomainService.Commands
{
    public class GuestCommandHandler : IGuestCommandHandler
    {
        private readonly IGuestService _service;

        public GuestCommandHandler(IGuestService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void HandleCommand(UpdateGuestCommand command)
        {
            _service.MoveGuest(command.GuestId, command.Longitude, command.Latitude);
        }
    }
}
