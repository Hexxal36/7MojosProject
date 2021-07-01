using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SourceOne.Services;
using System;
using System.Linq;

namespace SourceOne.Controllers
{
    [ApiController]
    [Route("/")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IWeatherForecastService _weatherForecastService;
        private readonly IHmacAuthenticationService _hmacAuthenticationService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWeatherForecastService weatherForecastService,
            IHmacAuthenticationService hmacAuthenticationService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
            _hmacAuthenticationService = hmacAuthenticationService;
        }

        [HttpGet]
        [Route("/today")]
        public IActionResult Get()
        {
            bool isAuth = false;

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                isAuth = _hmacAuthenticationService.IsValidRequest(
                    authHeader.Substring(0, 4),
                    authHeader.Substring(5),
                    Request.Method,
                    HttpContext.Request.GetDisplayUrl()); ;
            }
            catch { }

            var weather = _weatherForecastService.GetData();
            var statusCode = _weatherForecastService.GetStatusCode(isAuth);

            return _weatherForecastService.CreateResult(weather, statusCode);
        }
    }
}
