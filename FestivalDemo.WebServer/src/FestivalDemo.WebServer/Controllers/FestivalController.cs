using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestivalDemo.WebServer.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FestivalDemo.WebServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "Festival")]
    public class FestivalController
    {
        private readonly IFestivalService _festivalService;
        public FestivalController(IFestivalService festivalService)
        {
            _festivalService = festivalService;
        }
         
        [Route("open/{numberOfGuests}")]
        [HttpPost]
        public bool OpenFestival(int numberOfGuests)
        {
            return _festivalService.OpenFestival(numberOfGuests);
        }

        [Route("close")]
        [HttpPost]
        public bool CloseFestival()
        {
            return _festivalService.CloseFestival();
        }

    }
}
