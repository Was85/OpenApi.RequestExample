using DemoApi.Dtos;
using DemoApi.RequestExamples;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace DemoApi.Controllers;

[ApiController]
[Route("DefaultRoute")]

public class DefaultRouteController : ControllerBase
{
    [HttpPost]
    [RequestExample(typeof(BasicExamples))]
    public IActionResult Add(WeatherForecastDto  weatherForecastDto)
    {
        return Created("Get", new { });
    }
}