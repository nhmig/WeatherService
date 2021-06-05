using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Service;
using WeatherService.Service.Models;

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

        [HttpGet("temperature/{cityName}/{units}")]
        public async Task<ActionResult<ResponseTemperature>> GetTemperature(string cityName, string units)
        {
            var weatherTemperature = await _weatherService.GetTemperature(cityName, units);
            if (weatherTemperature is null)
            {
                return NotFound();
            }
            return weatherTemperature;
        }

        [HttpGet("wind/{cityName}")]
        public async Task<ActionResult<ResponseWind>> GetWind(string cityName)
        {
            var weatherWind = await _weatherService.GetWind(cityName);
            if (weatherWind is null)
            {
                return NotFound();
            }
            return new OkObjectResult(weatherWind);
        }

        [HttpGet("{cityName}/future/{units}")]
        public async Task<ActionResult<List<ResponseForecast>>> GetForecast5(string cityName, string units)
        {
            var forecast = await _weatherService.GetForecast5(cityName, units);
            if (forecast is null)
            {
                return NotFound("sdfsdf");
            }
            return new OkObjectResult(forecast);
        }
    }
}
