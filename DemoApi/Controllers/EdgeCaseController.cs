using DemoApi.Dtos;
using DemoApi.RequestExamples;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/edge")]
public class EdgeCaseController : ControllerBase
{
    // Edge Case: Missing Example Property
    [HttpPost("missing-example")]
    //[RequestExample(typeof(MissingExampleProvider))]
    //public IActionResult AddMissingExample(WeatherForecastDto weatherForecastDto)
    //{
    //    return Created("Get", new { });
    //}

    //// Edge Case: Non-Static Example Property
    //[HttpPost("non-static-example")]
    //[RequestExample(typeof(NonStaticExampleProvider))]
    //public IActionResult AddNonStaticExample(NonStaticExampleDto  nonStaticExampleDto)
    //{
    //    return Created("Get", new { });
    //}

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