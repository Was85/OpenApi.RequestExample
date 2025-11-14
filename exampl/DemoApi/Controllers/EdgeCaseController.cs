using DemoApi.Dtos;
using DemoApi.RequestExamples;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/edge")]
public class EdgeCaseController : ControllerBase
{
    // Duplicate Route Example
    [HttpPost("duplicate")]
    [RequestExample(typeof(BasicExamples), name: "DuplicateExample1")]
    public IActionResult AddDuplicate1(WeatherForecastDto  weatherForecastDto)
    {
        return Created("Get", new { });
    }

    [HttpPost("duplicate")]
    [RequestExample(typeof(NamedExamples), name: "DuplicateExample2")]
    public IActionResult AddDuplicate2(NamedExampleDto  namedExampleDto)
    {
        return Created("Get", new { });
    }
}