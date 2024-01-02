using System;

namespace WeatherApi.Models;

public class WeatherForecast
{
    public int TemperatureC { get; set; }

    public DateTime? Date { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
