using StackExchange.Redis;
using WebAppWithRedisCache;
using WebAppWithRedisCache.Cache;
using WebAppWithRedisCache.Interfaces;
using WebAppWithRedisCache.Models;
using WebAppWithRedisCache.Services;

var builder = WebApplication.CreateBuilder(args);

// Inject Redis cluster to the container
var redisCacheSettings = builder
                        .Configuration
                        .GetSection(nameof(RedisCacheSettings))
                        .Get<RedisCacheSettings>();
builder.Services.AddSingleton(redisCacheSettings);

if (redisCacheSettings.Enabled)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));
    builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
    builder.Services.AddSingleton<IResponseCacheService, RedisResponseCacheService>();
} 
else
{
    builder.Services.AddMemoryCache(options =>
    {
        // SizeLimit is used as CountLimit where the size of all entities is 1.
        // Support cache for up to 1000 concurrent loggers.
        options.SizeLimit = 1000;
        options.CompactionPercentage = 0.95;
    });
    builder.Services.AddSingleton<IResponseCacheService, InMemoryResponseCacheService>();
}

// Add services to the container.
builder.Services.AddControllers();

var openWeatherApiOptions = builder
                           .Configuration
                           .GetSection(nameof(OpenWeatherApiOptions))
                           .Get<OpenWeatherApiOptions>();

builder.Services.AddSingleton(openWeatherApiOptions);

builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();
builder.Services.AddHttpClient<IWeatherClient, OpenWeatherClient>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
