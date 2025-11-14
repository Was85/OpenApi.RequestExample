using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace DemoApi.RequestExamples;

// Example Providers

public static class OverwriteExamples
{
    public static IOpenApiExample Example => new OpenApiExample
    {
        Summary = "Overwrite Example",
        Value = new JsonObject
        {
            ["Date"] = "2023-01-01",
            ["Summary"] = "Stormy"
        }
    };
}

