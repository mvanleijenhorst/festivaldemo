using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.Common.Commands;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.DomainService.Commands
{
    public class GuestCommandDispatcher : IGuestCommandDispatcher
    {
        // service bus is the delegate listener
        private readonly IServiceBusAdapter _serviceBus;

        public GuestCommandDispatcher(IServiceBusAdapter serviceBus)
        {
            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        public void Dispatch(AddGuestCommand command)
        {
            _serviceBus.AddMessage(QueueConstant.OutgoingQueue, command);
        }

        public void Dispatch(RemoveGuestCommand command)
        {
            _serviceBus.AddMessage(QueueConstant.OutgoingQueue, command);
        }
    }
}
