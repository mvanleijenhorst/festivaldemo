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
    public class TrafficController
    {
        private readonly IGuestService _service;

        public TrafficController(IGuestService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Guest> AllGuests()
        {
            return _service.GetAllGuests();
        }

        [HttpGet]
        [Route("/ticket/{id}")]
        public Guest Guest(int id)
        {
            var guest = _service.GetAllGuests().FirstOrDefault(g => g.Ticket.Id == id);
            if (guest == null)
            { 
                throw new KeyNotFoundException($"Guest with ticket '{id}' not found.");
            }

            return guest;
        }
    }
}
