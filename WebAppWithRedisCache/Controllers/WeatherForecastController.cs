using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using WebAppWithRedisCache.Cache;
using WebAppWithRedisCache.Interfaces;
using WebAppWithRedisCache.Models;

namespace WebAppWithRedisCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherClient _weatherClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherClient weatherClient)
        {
            _logger = logger;
            _weatherClient = weatherClient;
        }

        [HttpGet(Name = "weather/{city}")]
        [Cached(600)]
        public async Task<IActionResult> Forecast(string city)
        {
            var weather = await _weatherClient.GetCurrentWeatherForCity(city);
            return weather is not null ? Ok(weather) : NotFound();
        }
    }
}