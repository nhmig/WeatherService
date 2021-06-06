using Refit;

namespace OpenWeatherMap.Client.Models
{
    public class RequestWeatherParam
    {
        [AliasAs("q")]
        public string cityName { get; set; }
        [AliasAs("units")]
        public string metric { get; set; }
        [AliasAs("token")]
        public string appid { get; set; }
    }
}
