using DemoApi.Dtos;
using DemoApi.RequestExamples;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace DemoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BasicController : ControllerBase
{
    [HttpPost]
    [RequestExample(typeof(BasicExamples))]
    public IActionResult AddBasic(WeatherForecastDto weatherForecastDto )
    {
        return Created("Get", new { });
    }

    [HttpGet("get-item")]
    [RequestExample(typeof(NamedExamples), name: "GetItemExample")]
    public IActionResult GetItem(NamedExampleDto  namedExampleDto)
    {
        return Ok(new { });
    }
}