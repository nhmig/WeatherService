using System;

namespace WeatherService.Service.Models
{
    public class ResponseForecast
    {
        public DateTime date { get; set; }
        public string city { get; set; }
        public float temperature { get; set; }
        public string temperatureMetric { get; set; }
    }
}
