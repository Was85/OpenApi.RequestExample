namespace DemoApi.RequestExamples;

public static class NamedExamples
{
    public static Microsoft.OpenApi.Models.OpenApiExample Example => new Microsoft.OpenApi.Models.OpenApiExample
    {
        Summary = "Named Example",
        Value = new Microsoft.OpenApi.Any.OpenApiObject
        {
            ["Date"] = new Microsoft.OpenApi.Any.OpenApiString("2022-01-01"),
            ["Summary"] = new Microsoft.OpenApi.Any.OpenApiString("Cloudy")
        }
    };
}