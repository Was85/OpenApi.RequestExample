namespace DemoApi.RequestExamples;

public static class BasicExamples
{
    public static IDictionary<string, Microsoft.OpenApi.Models.OpenApiExample> Example => new Dictionary<string, Microsoft.OpenApi.Models.OpenApiExample>
    {
        ["SunnyExample"] = new Microsoft.OpenApi.Models.OpenApiExample
        {
            Summary = "Sunny Weather",
            Value = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["Date"] = new Microsoft.OpenApi.Any.OpenApiString("2021-07-01"),
                ["Summary"] = new Microsoft.OpenApi.Any.OpenApiString("Sunny"),
            }
        },
        ["RainyExample"] = new Microsoft.OpenApi.Models.OpenApiExample
        {
            Summary = "Rainy Weather",
            Value = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["Date"] = new Microsoft.OpenApi.Any.OpenApiString("2021-07-02"),
                ["Summary"] = new Microsoft.OpenApi.Any.OpenApiString("Rainy"),
            }
        }
    };
}