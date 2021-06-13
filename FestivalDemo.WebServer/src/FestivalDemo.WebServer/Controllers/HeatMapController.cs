using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FestivalDemo.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeatmapController : ControllerBase
    {
        private readonly ILogger<HeatmapController> _logger;

        public HeatmapController(ILogger<HeatmapController> logger)
        {
            _logger = logger;
        }


    }
}
