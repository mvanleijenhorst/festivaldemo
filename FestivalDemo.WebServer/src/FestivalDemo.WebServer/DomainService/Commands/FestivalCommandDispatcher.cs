using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.Common.Commands;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.DomainService.Commands
{
    public class FestivalCommandDispatcher : IFestivalCommandDispatcher
    {
        // service bus is the delegate listener
        private readonly IServiceBusAdapter _serviceBus;

        public FestivalCommandDispatcher(IServiceBusAdapter serviceBus)
        {
            _serviceBus = serviceBus ?? throw new System.ArgumentNullException(nameof(serviceBus));
        }

        public void Dispatch(OpenFestivalCommand command)
        {
            _serviceBus.AddMessage(QueueConstant.OutgoingQueue, command);
        }

        public void Dispatch(CloseFestivalCommand command)
        {
            _serviceBus.AddMessage(QueueConstant.OutgoingQueue, command);
        }
    }
}
