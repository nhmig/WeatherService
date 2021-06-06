using OpenWeatherMap.Client.Models;
using Refit;
using System;
using System.Threading.Tasks;

namespace OpenWeatherMap.Client
{
    public interface IOpenWeatherMapClient
    {
        //api.openweathermap.org/data/2.5/weather?q=london&units=metric&appid=2caaefdcafd242c8eae17b5f7dc6337a
        [Get("/weather?q={cityName}&units={metric}&appid={token}")]
        //[Get("/weather")] // через класс ошибка, wtf? (RequestWeatherParam params)
        Task<ResponseCurrent> GetCurrentWeather(string cityName, string metric, string token);

        //api.openweathermap.org/data/2.5/forecast?q=London&units=metric&appid=2caaefdcafd242c8eae17b5f7dc6337a
        [Get("/forecast?q={cityName}&units={metric}&appid={token}")]
        Task<ResponseForecast5> GetForecastWeather(string cityName, string metric, string token);
    }
}
