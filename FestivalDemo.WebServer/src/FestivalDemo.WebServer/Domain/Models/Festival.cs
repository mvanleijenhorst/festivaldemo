using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FestivalDemo.WebServer.Domain.Models
{
    public class Festival
    {
        private readonly List<Ticket> _tickets;
        private readonly Dictionary<Ticket, Guest> _guests;
        private readonly List<Building> _buildings;

        public Festival()
        {
            _tickets = new List<Ticket>();
            _guests = new Dictionary<Ticket, Guest>();
            _buildings = new List<Building>();
        }

        public int MaxGuests { get; private set; }

        public ReadOnlyCollection<Guest> GuestList
        {
            get { return _guests.Values.ToList().AsReadOnly(); }
        }

        public void RegisterGuest(Guest guest)
        {
            var ticket = _tickets.FirstOrDefault(t => !_guests.ContainsKey(t));
            if (ticket != null)
            {
                if (_guests.TryAdd(ticket, guest))
                {
                    guest.GiveTicket(ticket);
                }
            }
        }

        public void UnregisterGuest(Guest guest)
        {
            var ticket = guest.Ticket;
            if (ticket != null && _guests.ContainsKey(ticket))
            {
                _guests.Remove(ticket);
            }
        }

        internal Ticket? GetTicket(int ticketId)
        {
            return _tickets.FirstOrDefault(t => t.Id == ticketId);
        }

        internal Guest? GetGuestByTicket(Ticket? ticket)
        {
            if (ticket == null)
            {
                return null;
            }

            if (_guests.ContainsKey(ticket))
            {
                return _guests[ticket];
            }

            return null;
        }

        public FestivalState State { get; private set; }

        public void Open(int maxGuests)
        {
            MaxGuests = maxGuests;

            for (int index = 0; index < MaxGuests; index++)
            {
                var ticket = new Ticket(index, Guid.NewGuid());
                _tickets.Add(ticket);
            }

            State = FestivalState.Open;
        }

        public void Close()
        {
            State = FestivalState.Closed;
            _guests.Clear();
            _buildings.Clear();
        }

        public IReadOnlyCollection<Building> Buildings { get { return _buildings.AsReadOnly(); } }

        public void AddBuilding(Building building)
        {
            Console.WriteLine($"Stand: {building.Type}");
            _buildings.Add(building);
        }


    }
}
