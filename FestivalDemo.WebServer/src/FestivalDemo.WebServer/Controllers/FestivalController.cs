using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FestivalDemo.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FestivalController
    {
        private readonly IFestivalService _festivalService;
        public FestivalController(IFestivalService festivalService)
        {
            _festivalService = festivalService;
        }
         
        [Route("open/{numberOfGuests}")]
        [HttpPost]
        public void OpenFestival(int numberOfGuests)
        {
            _festivalService.OpenFestival(numberOfGuests);
        }

        [Route("close")]
        [HttpPost]
        public void CloseFestival()
        {
            _festivalService.CloseFestival();
        }

    }
}
