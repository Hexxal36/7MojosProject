using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PublicAPI.Models;
using PublicAPI.Security;

namespace PublicAPI.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        public WeatherForecast GetData(Source[] sources)
        {
            var handler = new HmacClientHandler();
            var httpClient = HttpClientFactory.Create(handler);

            foreach (var source in sources.OrderBy(x => x.Priority))
            {
                try
                {
                    var response = httpClient.GetAsync(source.Url).Result;
                    var standardizedData = this.StandardizeData(response.Content.ReadAsStringAsync().Result);
                    if (!(standardizedData is null) && response.IsSuccessStatusCode)
                    {
                        return standardizedData;
                    }
                }
                catch { }
            }

            return null;
        }

        public WeatherForecast StandardizeData(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<WeatherForecast>(data);
            }
            catch { }

            try
            {
                var serializer = new XmlSerializer(typeof(Response));
                Response response;

                using (TextReader reader = new StringReader(data))
                {
                    response = (Response)serializer.Deserialize(reader);
                }

                var forecast = response.Data;

                return forecast;
            }
            catch { }

            return null;
        }
    }
}
