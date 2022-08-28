using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using WebAppWithRedisCache;
using WebAppWithRedisCache.Interfaces;
using WebAppWithRedisCache.Models;

var builder = WebApplication.CreateBuilder(args);
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
