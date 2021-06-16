using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Service.Models;
using WeatherService.Service.Services;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [SwaggerOperation(Summary = "Возвращает текущую температуру в указанном городе в цельсиях или фаренгейтах")]
        [HttpGet("temperature/{cityName}/{units}")]
        public async Task<ActionResult<ResponseTemperature>> GetTemperature(
            [FromRoute, SwaggerParameter("Название города", Required = true)] string cityName,
            [FromRoute, SwaggerParameter("Измерение: 'celsius' или 'fahrenheit'", Required = true)] string units)
        {
            var weatherTemperature = await _weatherService.GetTemperature(cityName, units);
            if (weatherTemperature is null)
            {
                return NotFound();
            }
            return weatherTemperature;
        }

        [SwaggerOperation(Summary = "Возвращает текущее направление и скорость ветра в указанном городе")]
        [HttpGet("wind/{cityName}")]
        public async Task<ActionResult<ResponseWind>> GetWind(
            [FromRoute, SwaggerParameter("Название города", Required = true)] string cityName)
        {
            var weatherWind = await _weatherService.GetWind(cityName);
            if (weatherWind is null)
            {
                return NotFound();
            }
            return weatherWind;
        }

        [SwaggerOperation(Summary = "Возвращает погоду на 5 дней вперед по указанному городу")]
        [HttpGet("{cityName}/future/{units}")]
        public async Task<ActionResult<List<ResponseForecast>>> GetForecast5(
            [FromRoute, SwaggerParameter("Название города", Required = true)] string cityName,
            [FromRoute, SwaggerParameter("Измерение: 'celsius' или 'fahrenheit'", Required = true)] string units)
        {
            var forecast = await _weatherService.GetForecast5(cityName, units);
            if (forecast is null)
            {
                return NotFound();
            }
            return forecast;
        }
    }
}
