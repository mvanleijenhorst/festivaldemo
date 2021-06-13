using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Models.Positions;
using FestivalDemo.WebServer.Domain.Repository;
using FestivalDemo.WebServer.DomainService.Commands;

namespace FestivalDemo.WebServer.Domain.Services
{
    public class GuestService: IGuestService
    {
        private readonly IFestivalRepository _repository;

        public GuestService(IFestivalRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void MoveGuest(int ticketId, float longitude, float latitude)
        {
            var festival = _repository.GetFestival();
            var ticket = festival.GetTicket(ticketId);

            if (ticket != null)
            {
                var guest = festival.GetGuestByTicket(ticket);
                if (guest != null)
                {
                    var coordinate = Coordinate.Create(longitude, latitude);
                    guest.Move(coordinate);
                }
            }
        }
    }
}
