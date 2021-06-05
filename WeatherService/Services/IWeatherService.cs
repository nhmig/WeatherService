using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Service.Models;

namespace WeatherService.Service
{
    public interface IWeatherService
    {
        string DegreeToDirection(int deg);
        Task<List<ResponseForecast>> GetForecast5(string cityName, string units);
        Task<ResponseTemperature> GetTemperature(string cityName, string units);
        Task<ResponseWind> GetWind(string cityName);
    }
}