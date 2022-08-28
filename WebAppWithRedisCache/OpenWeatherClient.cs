using WebAppWithRedisCache.Interfaces;
using WebAppWithRedisCache.Models;

namespace WebAppWithRedisCache
{
    public class OpenWeatherClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherApiOptions _openWeatherApiOptions;

        public OpenWeatherClient(HttpClient httpClient, OpenWeatherApiOptions openWeatherApiOptions)
        {
            _httpClient = httpClient;
            _openWeatherApiOptions = openWeatherApiOptions;
        }

        public async Task<WeatherResponse?> GetCurrentWeatherForCity(string city)
        {
            var weather = await _httpClient.GetFromJsonAsync<WeatherResponse>($"weather?q={city}&appid={_openWeatherApiOptions.ApiKey}");
            return weather;
        }
    }
}
