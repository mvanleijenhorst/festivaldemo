using System;
using FestivalDemo.WebServer.Domain.Models.Positions;

namespace FestivalDemo.WebServer.Domain.Models
{
    public class Guest
    {
        public Guest(GuestType type)
        {
            Type = type;

            PointOfinterest = Coordinate.PointZero;
            Position = Coordinate.PointZero;
        }

        public Ticket? Ticket { get; private set; }
        public GuestType Type { get; init; }

        public Coordinate PointOfinterest { get; set; }
        public Coordinate Position { get; private set; }

        public void Move(Coordinate position)
        {
            Position = position;
        }

        public void GiveTicket(Ticket ticket)
        {
            Ticket = ticket;
        }

    }
}
