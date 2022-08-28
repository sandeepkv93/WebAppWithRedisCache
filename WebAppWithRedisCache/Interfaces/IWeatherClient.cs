using WebAppWithRedisCache.Models;

namespace WebAppWithRedisCache.Interfaces
{
    public interface IWeatherClient
    {
        Task<WeatherResponse?> GetCurrentWeatherForCity(string city);
    }
}
