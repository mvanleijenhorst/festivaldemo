using System;
using System.Collections.Generic;
using System.Linq;
using FestivalDemo.WebServer.Domain.Models;
using FestivalDemo.WebServer.Domain.Models.Positions;
using FestivalDemo.WebServer.Domain.Repository;
using FestivalDemo.WebServer.DomainService.Commands;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.Domain.Services
{
    public class FestivalService : IFestivalService
    {
        private const float FollowerPercentage = 70.0f;
        private readonly IFestivalRepository _repository;
        private readonly IFestivalCommandDispatcher _festivalCommandDispatcher;
        private readonly IGuestCommandDispatcher _guestCommandDispatcher;

        public FestivalService(IFestivalRepository repository,
            IFestivalCommandDispatcher festivalCommandDispatcher,
            IGuestCommandDispatcher guestCommandDispatcher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _festivalCommandDispatcher = festivalCommandDispatcher ?? throw new ArgumentNullException(nameof(festivalCommandDispatcher));
            _guestCommandDispatcher = guestCommandDispatcher;
        }

        public bool OpenFestival(int numberOfGuests)
        {
            var festival = _repository.GetFestival();
            if (festival.State == FestivalState.Open)
            {
                return false;
            }

            festival.Open(numberOfGuests);

            _festivalCommandDispatcher.Dispatch(new OpenFestivalCommand());

            int maxFollowIndex = (int)(numberOfGuests / 100.0f * FollowerPercentage);
            for (int index = 0; index < numberOfGuests; index++)
            {
                var guestType = index < maxFollowIndex ? GuestType.Follower : GuestType.Misfit;
                var guest = new Guest(guestType);
                festival.RegisterGuest(guest);

                Console.WriteLine($"Guest account {guestType}");

                if (guest.Ticket != null)
                {
                    _guestCommandDispatcher.Dispatch(new AddGuestCommand(guest.Ticket.Id, guest.Type == GuestType.Follower));
                }
            }

            return true;
        }

        public bool CloseFestival()
        {
            var festival = _repository.GetFestival();
            if (festival.State == FestivalState.Closed)
            {
                return false;
            }

            var list = festival.GuestList;
            foreach (var guest in list)
            {
                if (guest.Ticket != null)
                {
                    festival.UnregisterGuest(guest);
                    _guestCommandDispatcher.Dispatch(new RemoveGuestCommand(guest.Ticket.Id));
                }
            }

            _festivalCommandDispatcher.Dispatch(new CloseFestivalCommand());
            festival.Close();

            return true;
        }

        public void BuildFestival(int buildingId, BuildingType buildingType, float longitude, float latitude)
        {
            var festival = _repository.GetFestival();

            var building = new Building(buildingId, buildingType, Coordinate.Create(longitude, latitude));
            festival.AddBuilding(building);
        }

        public IReadOnlyCollection<Building> GetBuildings()
        {
            var festival = _repository.GetFestival();
            return festival.Buildings;
        }
    }
}
