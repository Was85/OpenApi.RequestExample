using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace DemoApi.RequestExamples;

public static class TemperatureExamples
{
    public static IOpenApiExample Example => new OpenApiExample
    {
        Summary = "Temperature Example",
        Value = new JsonObject
        {
            ["Celsius"] = 25,
            ["Fahrenheit"] = 77
        }
    };
}