using SourceTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceTwo.Services
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

        public int GetError(bool isAuth)
        {
            if (!isAuth)
            {
                return 1;
            }

            var time = DateTime.UtcNow;
            if (time.Hour > 12)
            {
                return 3;
            }
            if (time.Second > 50)
            {
                return 2;
            }

            return 0;
        }

        public Response CreateResponse(WeatherForecast weather, int error)
        {
            return new Response()
            {
                Data = error == 0 ? weather : null,
                Success = error == 0,
                Error = error
            };
        }
    }
}
