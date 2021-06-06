using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;

namespace WeatherService.Client.Configuration
{
    public static class WeatherServiceClientConfiguration
    {
        public static IServiceCollection AddWeatherServiceClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<IWeatherClient>(new HttpClient()
            {
                BaseAddress = new Uri(configuration["ServiceUrls:WeatherService"])
            }));

            return services;
        }
    }
}
