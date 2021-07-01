using PublicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public interface IWeatherForecastService
    {
        public WeatherForecast GetData(Source[] sources);

        public WeatherForecast StandardizeData(string data);
    }
}
