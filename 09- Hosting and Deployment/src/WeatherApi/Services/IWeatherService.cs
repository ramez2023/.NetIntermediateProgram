using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherService
{
    Task<WeatherForecast[]> GetWeatherForecasts();
}