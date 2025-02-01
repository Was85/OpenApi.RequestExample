using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;
using WebApi_OpenApiExampleDemo.Controllers;

namespace DemoApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static List<string> Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [RequestExample(typeof(WeatherForecastExamples),name: "WeatherForecast Examples")]
    public IEnumerable<WeatherForecast> Get(WeatherForecast weatherForecast)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Count)]
            })
            .ToArray();
    }

    [HttpPost]
    [RequestExample(typeof(WeatherForecastExamples), name: "WeatherForecast Examples")]
    public IActionResult Add(WeatherForecast weatherForecast)
    {
        Summaries.Add(weatherForecast.Summary);

        return Created("Get",weatherForecast);
    }
}
