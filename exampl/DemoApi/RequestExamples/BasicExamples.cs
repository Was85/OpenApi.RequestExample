using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace DemoApi.RequestExamples;

public static class BasicExamples
{
    public static IDictionary<string, IOpenApiExample> Example => new Dictionary<string, IOpenApiExample>
    {
        ["SunnyExample"] = new OpenApiExample
        {
            Summary = "Sunny Weather",
            Value = new JsonObject
            {
                ["Date"] = "2021-07-01",
                ["Summary"] = "Sunny",
            }
        },
        ["RainyExample"] = new OpenApiExample
        {
            Summary = "Rainy Weather",
            Value = new JsonObject
            {
                ["Date"] = "2021-07-02",
                ["Summary"] = "Rainy",
            }
        }
    };
}