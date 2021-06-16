using System.Collections.Generic;
using FestivalDemo.WebServer.Domain.Models;
using FestivalDemo.WebServer.Domain.Models.Positions;

namespace FestivalDemo.WebServer.Domain.Services
{
    public interface IGuestService
    {
        void MoveGuest(int guestId, float longitude, float latitude);
        Coordinate GetHotSpot();
        IEnumerable<Guest> GetAllGuests();
    }
}
