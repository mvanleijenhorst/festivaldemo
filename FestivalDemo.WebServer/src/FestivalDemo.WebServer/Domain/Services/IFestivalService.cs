using System.Collections.Generic;
using FestivalDemo.WebServer.Domain.Models;

namespace FestivalDemo.WebServer.Domain.Services
{
    public interface IFestivalService
    {
        IReadOnlyCollection<Building> GetBuildings();
        void BuildFestival(int buildingId, BuildingType buildingType, float longitude, float latitude);

        void OpenFestival(int numberOfGuests);
        void CloseFestival();
    }
}
