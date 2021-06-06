using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;

namespace OpenWeatherMap.Client.Configuration
{
    public static class OpenWeatherMapClientConfiguration
    {
        public static IServiceCollection AddOpenWeatherMapServiceClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<IOpenWeatherMapClient>(new HttpClient()
            {
                BaseAddress = new Uri(configuration["PublicApi:OpenWeatherMap"])
            }));

            return services;
        }
    }
}
