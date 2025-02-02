using DemoApi.Dtos;
using DemoApi.RequestExamples;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/allmethods")]
public class AllMethodsController : ControllerBase
{
    [HttpGet]
    [RequestExample(typeof(BasicExamples), name: "GetExample")]
    public IActionResult GetItem(GeneralDto generalDto)
    {
        return Ok(new { });
    }

    [HttpPost]
    [RequestExample(typeof(BasicExamples), name: "PostExample")]
    public IActionResult AddItem(GeneralDto generalDto)
    {
        return Created("Get", new { });
    }

    [HttpPut]
    [RequestExample(typeof(BasicExamples), name: "PutExample")]
    public IActionResult UpdateItem(GeneralDto generalDto)
    {
        return Ok(new { });
    }

    [HttpDelete]
    [RequestExample(typeof(BasicExamples), name: "DeleteExample")]
    public IActionResult DeleteItem(GeneralDto generalDto)
    {
        return NoContent();
    }

    [HttpPatch]
    [RequestExample(typeof(BasicExamples), name: "PatchExample")]
    public IActionResult PatchItem(GeneralDto generalDto)
    {
        return Ok(new { });
    }

    [HttpHead]
    [RequestExample(typeof(BasicExamples), name: "HeadExample")]
    public IActionResult HeadItem(GeneralDto generalDto)
    {
        return Ok(new { });
    }

    [HttpOptions]
    [RequestExample(typeof(BasicExamples), name: "OptionsExample")]
    public IActionResult OptionsItem(GeneralDto generalDto)
    {
        return Ok(new { });
    }
}

