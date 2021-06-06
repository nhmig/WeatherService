using OpenWeatherMap.Client.Models;
using OpenWeatherMap.Client.Models5;
using Refit;
using System;
using System.Threading.Tasks;

namespace OpenWeatherMap.Client
{
    public interface IOpenWeatherMapClient
    {
        //api.openweathermap.org/data/2.5/weather?q=london&units=metric&appid=2caaefdcafd242c8eae17b5f7dc6337a
        [Get("/weather?q={cityName}&units={metric}&appid=2caaefdcafd242c8eae17b5f7dc6337a")]
        Task<ResponseCurrent> GetCurrentWeather(string cityName, string metric);

        //api.openweathermap.org/data/2.5/forecast?q=London&units=metric&appid=2caaefdcafd242c8eae17b5f7dc6337a
        [Get("/forecast?q={cityName}&units={metric}&appid=2caaefdcafd242c8eae17b5f7dc6337a")]
        Task<ResponseForecast5> GetForecastWeather(string cityName, string metric);
    }
}
