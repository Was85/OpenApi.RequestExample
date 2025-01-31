using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;
using WebApi_OpenApiExampleDemo.Controllers;

namespace DemoApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name="test2")]
    [RequestExample(typeof(WeatherForecastExamples), nameof(WeatherForecastExamples.Example), "WeatherForecast Examples")]
    public IEnumerable<WeatherForecast> Kalle(WeatherForecast weatherForecast)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("test")]
    [RequestExample(typeof(WeatherForecastExamples), nameof(WeatherForecastExamples.Example), "WeatherForecast Examples")]
    public IEnumerable<WeatherForecast> Kalle2()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    //// Another get random number
    //[HttpGet("random")]
    //[RequestExample(typeof(WeatherForecastExamples), nameof(WeatherForecastExamples.Example), "WeatherForecast Examples")]
    //public int RandomNumber()
    //{
    //    return Random.Shared.Next();
    //}

}
