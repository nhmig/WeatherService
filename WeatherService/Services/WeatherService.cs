using AutoMapper;
using System;
using System.Threading.Tasks;
using WeatherService.Service.Models;
using System.Collections.Generic;
using OpenWeatherMap.Client;
using Refit;
using OpenWeatherMap.Client.Models;
using Microsoft.Extensions.Configuration;

namespace WeatherService.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IMapper _mapper;
        private readonly IOpenWeatherMapClient _openWeatherMapClient;
        private readonly IConfiguration _configuration;

        public WeatherService(IMapper mapper, IOpenWeatherMapClient openWeatherMapClient, IConfiguration configuration)
        {
            _mapper = mapper;
            _openWeatherMapClient = openWeatherMapClient;
            _configuration = configuration;
        }

        public async Task<ResponseTemperature> GetTemperature(string cityName, string units)
        {
            string metric = UnitsToMetric(units);

            if (cityName == null || metric == null)
            {
                return null;
            }
            ResponseCurrent resultingMessage;
            try
            {
                resultingMessage = await _openWeatherMapClient.GetCurrentWeather(cityName, metric, _configuration["token"]);
            }
            catch (ApiException ex)
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

            ResponseCurrent resultingMessage;
            try
            {
                resultingMessage = await _openWeatherMapClient.GetCurrentWeather(cityName, "metric", _configuration["token"]);
            }
            catch (ApiException ex)
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

            ResponseForecast5 resultingMessage;
            try
            {
                resultingMessage = await _openWeatherMapClient.GetForecastWeather(cityName, metric, _configuration["token"]);
            }
            catch (ApiException ex)
            {
                return null;
            }

            //только по первому значению из списка (отсортированного) для каждой даты
            var result = new List<ResponseForecast>();
            var iDate = DateTime.UtcNow.Date;
            DateTime jDate;
            foreach (var item in resultingMessage.list)
            {
                jDate = DateTime.Parse(item.dt_txt).Date;
                if (iDate == jDate)
                {
                    continue;
                }
                iDate = jDate;
                result.Add(new ResponseForecast
                {
                    date = jDate.ToString("yyyy-MM-dd"),
                    city = cityName,
                    temperature = item.main.temp,
                    temperatureMetric = units,
                });
            }

            //return _mapper.Map<ResponseForecast>(result);
            return result;
        }

        private string UnitsToMetric(string units) => units switch
        {
            "celsius" => "metric",
            "fahrenheit" => "imperial",
            _ => null
        };

    }
}
