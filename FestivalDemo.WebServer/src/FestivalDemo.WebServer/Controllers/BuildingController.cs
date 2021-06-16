using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Models;
using FestivalDemo.WebServer.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FestivalDemo.WebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController
    {
        private readonly IFestivalService _festivalService;
        public BuildingController(IFestivalService festivalService)
        {
            _festivalService = festivalService;
        }

        [Route("type/{type}")]
        [HttpGet]
        public IEnumerable<Building> GetBuildingByType(string buildingType)
        {
            if (Enum.TryParse<BuildingType>(buildingType, out var type))
            {
                _festivalService
                    .GetBuildings()
                    .Where(b => b.Type == type)
                    .ToList();
            }

            return Array.Empty<Building>();
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Building> GetBuildings()
        {
            return _festivalService
                .GetBuildings()
                .ToList();
        }

        [Route("{id}")]
        [HttpGet]
        public Building GetBuildingById(int id)
        {
            var building = _festivalService
                .GetBuildings()
                .FirstOrDefault(b => b.Id == id);

            if (building == null)
            { 
                throw new KeyNotFoundException($"Building with id '{id}' not found.");
            }

            return building;
        }

    }
}
