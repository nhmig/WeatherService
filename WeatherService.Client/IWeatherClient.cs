using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Service.Models;

namespace WeatherService.Client
{
    public interface IWeatherClient
    {
        [Get("temperature/{cityName}/{units}")]
        Task<ResponseTemperature> GetTemperature(string cityName, string units);

        [Get("wind/{cityName}")]
        Task<ResponseWind> GetWind(string cityName);

        [Get("{cityName}/future/{units}")]
        Task<List<ResponseForecast>> GetForecast5(string cityName, string units);
    }
}
