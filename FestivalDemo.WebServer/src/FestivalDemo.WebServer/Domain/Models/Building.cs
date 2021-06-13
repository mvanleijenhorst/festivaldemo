using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Models.Positions;

namespace FestivalDemo.WebServer.Domain.Models
{
    public class Building
    {
        public Building(int id, BuildingType type, Coordinate location)
        {
            Id = id;
            Type = type;
            Location = location;
        }

        public int Id { get; init; }

        public BuildingType Type { get; init; }

        public Coordinate Location { get; init; }
    }
}
