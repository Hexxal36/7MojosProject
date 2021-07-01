using Microsoft.AspNetCore.Mvc;
using SourceOne.Models;

namespace SourceOne.Services
{
    public interface IWeatherForecastService
    {
        public WeatherForecast GetData();

        public IActionResult CreateResult(WeatherForecast weather, int statusCode);

        public int GetStatusCode(bool isAuth);
    }
}
