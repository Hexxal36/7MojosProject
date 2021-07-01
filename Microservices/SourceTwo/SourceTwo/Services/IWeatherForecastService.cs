using SourceTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceTwo.Services
{
    public interface IWeatherForecastService
    {
        public WeatherForecast GetData();

        public int GetError(bool isAuth);

        public Response CreateResponse(WeatherForecast weather,
            int error);
    }
}
