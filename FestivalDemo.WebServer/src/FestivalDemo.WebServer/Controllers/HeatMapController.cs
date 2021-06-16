using FestivalDemo.WebServer.Domain.Models.Positions;
using FestivalDemo.WebServer.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FestivalDemo.WebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeatmapController : ControllerBase
    {
        private readonly ILogger<HeatmapController> _logger;
        private readonly IGuestService _guestService;

        public HeatmapController(ILogger<HeatmapController> logger, IGuestService guestService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
        }
        
        [HttpGet]
        [Route("/hotspot")]
        public Coordinate HotSpot()
        {
            return _guestService.GetHotSpot();
        }

    }
}
