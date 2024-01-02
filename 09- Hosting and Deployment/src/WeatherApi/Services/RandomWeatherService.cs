using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services;

public class RandomWeatherService : IWeatherService
{
    private static readonly string[] Summaries = {  "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    public Task<WeatherForecast[]> GetWeatherForecasts()
    {
        var weatherForecasts = Enumerable
            .Range(1, 5)
            .Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
            .ToArray();

        return Task.FromResult(weatherForecasts);
    }
}