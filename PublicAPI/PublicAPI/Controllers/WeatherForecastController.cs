using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ISourceService _sourceService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService,
            ISourceService sourceService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
            _sourceService = sourceService;
        }

        [HttpGet]
        public IActionResult Get([FromHeader] int preffered)
        {
            var sources = _sourceService.GetSources(preffered);
            var data = _weatherForecastService.GetData(sources);

            if (data is null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(data);
        }
    }
}
