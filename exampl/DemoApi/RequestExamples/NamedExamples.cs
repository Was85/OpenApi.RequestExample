using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace DemoApi.RequestExamples;

public static class NamedExamples
{
    public static IOpenApiExample Example => new OpenApiExample
    {
        Summary = "Named Example",
        Value = new JsonObject
        {
            ["Date"] = "2022-01-01",
            ["Summary"] = "Cloudy"
        }
    };
}