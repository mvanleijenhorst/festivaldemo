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
        private const float _followerPercentage = 95.0f;
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

        public void OpenFestival(int numberOfGuests)
        {
            var festival = _repository.GetFestival();
            festival.Open(numberOfGuests);

            _festivalCommandDispatcher.Dispatch(new OpenFestivalCommand());

            for (int index = 0; index < numberOfGuests; index++)
            {
                var guest = CreateGuest(festival.GuestList);
                festival.RegisterGuest(guest);

                if (guest.Ticket != null)
                {
                    _guestCommandDispatcher.Dispatch(new AddGuestCommand(guest.Ticket.Id, guest.Type == GuestType.Follower));
                }
            }
        }

        public void CloseFestival()
        {
            var festival = _repository.GetFestival();
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

        private static Guest CreateGuest(ICollection<Guest> list)
        {
            var totalGuest = list.Count + 1;
            var numberOfNewFollowers = (int)(totalGuest / _followerPercentage);
            var numberOfCurrentFollowers = list.Where(g => g.Type == GuestType.Follower).Count();

            var guestType = numberOfNewFollowers > numberOfCurrentFollowers ? GuestType.Follower : GuestType.Misfit;

            return new Guest(guestType);
        }
    }
}
