using DemoApi.Dtos;
using DemoApi.RequestExamples;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/advanced")]
public class AdvancedController : ControllerBase
{
    [HttpPut]
    [RequestExample(typeof(OverwriteExamples), overwriteExisting: true)]
    public IActionResult UpdateItem(OverwriteExampleDto overwriteExampleDto)
    {
        return Ok(new { });
    }

    [HttpDelete("{id}")]
    [RequestExample(typeof(OverwriteExamples), name: "DeleteExample", overwriteExisting: true)]
    public IActionResult DeleteItem(OverwriteExampleDto overwriteExampleDto)
    {
        return NoContent();
    }

    [HttpPatch("{id}")]
    [RequestExample(typeof(NamedExamples), name: "PatchExample")]
    public IActionResult PatchItem(NamedExampleDto namedExampleDto)
    {
        return Ok(new { });
    }
}