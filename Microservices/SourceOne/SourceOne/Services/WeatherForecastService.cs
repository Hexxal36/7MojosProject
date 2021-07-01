using Microsoft.AspNetCore.Mvc;
using SourceOne.Models;
using SourceOne.Services;
using System;

namespace SourceOne.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        public WeatherForecast GetData()
        {
            var rng = new Random();
            var weather = new WeatherForecast
            {
                Temperature = rng.Next(0, 5000) / 100.0,
                Pressure = rng.Next(0, 6000) / 100.0
            };

            return weather;
        }

        public int GetStatusCode(bool isAuth)
        {
            if (!isAuth)
            {
                return 401;
            }

            var time = DateTime.UtcNow;
            if (time.Hour <= 8)
            {
                return 503;
            }

            if (time.Second < 10)
            {
                return 500;
            }

            return 200;
        }

        public IActionResult CreateResult(WeatherForecast weather, int statusCode)
        {
            if (statusCode == 200)
            {
                var result = new JsonResult(weather);
                return result;
            }

            return new StatusCodeResult(statusCode);
        }
    }
}
