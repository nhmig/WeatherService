using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherService.Service.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WeatherService.Service.Models.openweathermap.org;

namespace WeatherService.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public WeatherService(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ResponseTemperature> GetTemperature(string cityName, string units)
        {
            string metric = UnitsToMetric(units);

            if (cityName == null || metric == null)
            {
                return null;
            }

            ResponseCurrent resultingMessage = await RequestCurrentWeatherApi(cityName, metric);
            if (resultingMessage == null)
            {
                return null;
            }

            var result  = _mapper.Map<ResponseTemperature>(resultingMessage);
            result.metric = units;
            return result;
        }

        public async Task<ResponseWind> GetWind(string cityName)
        {
            if (cityName == null)
            {
                return null;
            }

            ResponseCurrent resultingMessage = await RequestCurrentWeatherApi(cityName, "metric");
            if (resultingMessage == null)
            {
                return null;
            }

            return _mapper.Map<ResponseWind>(resultingMessage);
        }

        public async Task<List<ResponseForecast>> GetForecast5(string cityName, string units)
        {
            string metric = UnitsToMetric(units);

            if (cityName == null || metric == null)
            {
                return null;
            }

            ResponseForecast5 resultingMessage = await RequestForecastWeatherApi(cityName, "metric");
            if (resultingMessage == null)
            {
                return null;
            }

            var result = new List<ResponseForecast>();
            foreach (var item in resultingMessage.list)
            {
                result.Add(new ResponseForecast
                {
                    date = DateTime.Parse(item.dt_txt),
                    city = cityName,
                    temperature = item.main.temp,
                    temperatureMetric = units,
                });
            }

            //return _mapper.Map<ResponseForecast>(result);
            return result;
        }

        private async Task<ResponseCurrent> RequestCurrentWeatherApi(string cityName, string metric)
        {
            string url = $"{_configuration["PublicApi:WheatherCurrent"]}" +
                $"?q={cityName}" +
                $"&units={metric}" +
                $"&appid={_configuration["token"]}";

            ResponseCurrent resultingMessage;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    resultingMessage = JsonConvert.DeserializeObject<ResponseCurrent>(apiResponse);
                }
            }

            return resultingMessage;
        }

        private async Task<ResponseForecast5> RequestForecastWeatherApi(string cityName, string metric)
        {
            string url = $"{_configuration["PublicApi:WheatherForecast"]}" +
                $"?q={cityName}" +
                $"&units={metric}" +
                $"&appid={_configuration["token"]}";

            ResponseForecast5 resultingMessage;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    resultingMessage = JsonConvert.DeserializeObject<ResponseForecast5>(apiResponse);
                }
            }

            return resultingMessage;
        }

        private string UnitsToMetric(string units) => units switch
        {
            "celsius" => "metric",
            "fahrenheit" => "imperial",
            _ => null
        };
        
        //public string DegreeToDirection(int deg) => deg switch
        //{
        //    int when deg > 337.5 || deg < 22.5 => "North",
        //    int when deg < 67.5 => "Northeast",
        //    int when deg < 112.5 => "East",
        //    int when deg < 157.5 => "Southeast",
        //    int when deg < 202.5 => "South",
        //    int when deg < 247.5 => "Southwest",
        //    int when deg < 293.5 => "West",
        //    _ => "Northwest"
        //};
    }
}
