using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Models;
using FestivalDemo.WebServer.Domain.Services;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.DomainService.Commands
{
    public class FestivalCommandHandler : IFestivalCommandHandler
    {
        private readonly IFestivalService _service;

        public FestivalCommandHandler(IFestivalService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));    
        }

        public void HandleCommand(BuildingInfoCommand command)
        {
            _service.BuildFestival(command.BuildingId, (BuildingType)command.BuildingType, command.Longitude, command.Latitude);
        }
    }
}
