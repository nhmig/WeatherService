using AutoMapper;
using WeatherService.Service.Models;
using openweathermap = WeatherService.Service.Models.openweathermap.org;

namespace WeatherService.Service.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ResponseCurrent, ResponseTemperature> ()
                .ForMember(dest => dest.city, act => act.MapFrom(src => src.name))
                .ForMember(dest => dest.temperature, act => act.MapFrom(src => src.main.temp));
                //.ForMember(dest => dest.metric, act => act.MapFrom(src => src.));

            CreateMap<ResponseCurrent, ResponseWind>()
                .ForMember(dest => dest.city, act => act.MapFrom(src => src.name))
                .ForMember(dest => dest.speed, act => act.MapFrom(src => src.wind.speed))
                .ForMember(dest => dest.direction, act => act.MapFrom(src => DegreeToDirection(src.wind.deg)));

            //CreateMap<openweathermap.List, ResponseForecast>()
            //    .ForMember(dest => dest.date, act => act.MapFrom(src => src.dt_txt))
            //    .ForMember(dest => dest.city, act => act.MapFrom(src => src.))
            //    .ForMember(dest => dest.temperature, act => act.MapFrom(src => src.main.temp))
            //    .ForMember(dest => dest.temperatureMetric, act => act.MapFrom(src => src.));
        }

        public string DegreeToDirection(int deg) => deg switch
        {
            int when deg > 337.5 || deg < 22.5 => "North",
            int when deg < 67.5 => "Northeast",
            int when deg < 112.5 => "East",
            int when deg < 157.5 => "Southeast",
            int when deg < 202.5 => "South",
            int when deg < 247.5 => "Southwest",
            int when deg < 293.5 => "West",
            _ => "Northwest"
        };
    }
}
