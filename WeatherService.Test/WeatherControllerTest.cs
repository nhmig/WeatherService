using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OpenWeatherMap.Client;
using OpenWeatherMap.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService.Controllers;
using WeatherService.Service.Configuration;
using WeatherService.Service.Models;
using WeatherService.Service.Services;
using Xunit;

namespace WeatherService.Test
{
    public class WeatherControllerTest
    {
        private readonly Mock<IOpenWeatherMapClient> _openWeatherMapClient = new Mock<IOpenWeatherMapClient>();
        private readonly IMapper _mapper;
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        public WeatherControllerTest()
        {
            _mapper = Mapper();
            //(IMapper mapper, IOpenWeatherMapClient openWeatherMapClient, IConfiguration configuration)
            _weatherService = new Service.Services.WeatherService(_mapper, _openWeatherMapClient.Object, _configuration.Object);
            _logger = Mock.Of<ILogger<WeatherController>>();
        }

        public IMapper Mapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            return config.CreateMapper();
        }

        [Fact]
        public async Task WeatherController_GetCurrentWeather_ShouldExecute()
        {
            //Arrange
            string cityName = "London";
            string units = "celsius";
            var response = new ResponseTemperature
            {
                city = cityName,
                metric = units,
                temperature = 25
            };
            var responseOpenWeatherMapClient = new ResponseCurrent
            {
                name = cityName,
                main = new Main 
                {
                    temp = 25
                }
            };

            _openWeatherMapClient.Setup(x => x.GetCurrentWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseOpenWeatherMapClient);
            var controller = new WeatherController(_logger, _weatherService);

            //Act
            var actionResult = await controller.GetTemperature(cityName, units);

            //Assert
            Assert.NotNull(actionResult.Value);
            Assert.Equal(response.city, actionResult.Value.city);
            Assert.Equal(response.metric, actionResult.Value.metric);
            Assert.Equal(response.temperature, actionResult.Value.temperature);
            _openWeatherMapClient.Verify(x => x.GetCurrentWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task WeatherController_GetWind_ShouldExecute()
        {
            //Arrange
            string cityName = "London";
            var response = new ResponseWind
            {
                city = cityName,
                speed = 10,
                direction = "North"
            };
            var responseOpenWeatherMapClient = new ResponseCurrent
            {
                name = cityName,
                wind = new Wind
                {
                    deg = 0,
                    speed = 10
                }
            };

            _openWeatherMapClient.Setup(x => x.GetCurrentWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseOpenWeatherMapClient);
            var controller = new WeatherController(_logger, _weatherService);

            //Act
            var actionResult = await controller.GetWind(cityName);

            //Assert
            Assert.NotNull(actionResult.Value);
            Assert.Equal(response.city, actionResult.Value.city);
            Assert.Equal(response.speed, actionResult.Value.speed);
            Assert.Equal(response.direction, actionResult.Value.direction);
            _openWeatherMapClient.Verify(x => x.GetCurrentWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task WeatherController_GetForecast5_ShouldExecute()
        {
            //Arrange
            string cityName = "London";
            string units = "celsius";
            var response = new List<ResponseForecast>() {
                new ResponseForecast
                {
                    city = cityName,
                    date = "2022-06-06",
                    temperature = 15,
                    temperatureMetric = units
                },
                new ResponseForecast
                {
                    city = cityName,
                    date = "2022-06-07",
                    temperature = 20,
                    temperatureMetric = units
                }
            };

            var responseOpenWeatherMapClient = new ResponseForecast5
            {
                list = new List[]
                {
                    new List
                    {
                        dt_txt = "2022-06-06 12:00:00",
                        main =  new Main {temp = 15}
                    },
                    new List
                    {
                        dt_txt = "2022-06-07 12:00:00",
                        main =  new Main {temp = 20}
                    }
                }
            };

            _openWeatherMapClient.Setup(x => x.GetForecastWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(responseOpenWeatherMapClient);
            var controller = new WeatherController(_logger, _weatherService);

            //Act
            var actionResult = await controller.GetForecast5(cityName, units);

            //Assert
            Assert.NotNull(actionResult.Value);
            Assert.Equal(response[0].city, actionResult.Value[0].city);
            Assert.Equal(response[0].temperature, actionResult.Value[0].temperature);
            Assert.Equal(response[0].date, actionResult.Value[0].date);
            Assert.Equal(response[1].temperatureMetric, actionResult.Value[1].temperatureMetric);
            Assert.Equal(response[1].temperature, actionResult.Value[1].temperature);
            Assert.Equal(response[1].date, actionResult.Value[1].date);
            _openWeatherMapClient.Verify(x => x.GetForecastWeather(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
