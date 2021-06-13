using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Common;
using FestivalDemo.WebServer.DomainService.Adapters;
using FestivalDemo.WebServer.DomainService.Commands;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FestivalDemo.WebServer.BackgroundServices
{
    public class ListenerBackgroundService : BackgroundService
    {
        private readonly IServiceBusAdapter _adapter;
        private readonly IFestivalCommandHandler _festivalCommandHandler;
        private readonly IGuestCommandHandler _guestCommandHandler;

        public ListenerBackgroundService(
            IServiceBusAdapter adapter,
            IFestivalCommandHandler festivalCommandHandler,
            IGuestCommandHandler guestCommandHandler)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _festivalCommandHandler = festivalCommandHandler ?? throw new ArgumentNullException(nameof(festivalCommandHandler));
            _guestCommandHandler = guestCommandHandler ?? throw new ArgumentNullException(nameof(guestCommandHandler));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_adapter.TryGetMessage(ConfigurationConstants.IncomingQueue, out var message))
                {
                    switch (message)
                    {
                        case UpdateGuestCommand cmd : _guestCommandHandler.HandleCommand(cmd);
                            break;
                        case BuildingInfoCommand cmd: _festivalCommandHandler.HandleCommand(cmd);
                            break;
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
