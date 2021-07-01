using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceTwo.Models
{
    public class Response
    {
        public bool Success { get; set; }

        public int Error { get; set; }

        public WeatherForecast Data { get; set; }
    }
}
