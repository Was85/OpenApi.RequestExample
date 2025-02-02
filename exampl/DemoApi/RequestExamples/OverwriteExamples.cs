namespace DemoApi.RequestExamples;

// Example Providers

public static class OverwriteExamples
{
    public static Microsoft.OpenApi.Models.OpenApiExample Example => new Microsoft.OpenApi.Models.OpenApiExample
    {
        Summary = "Overwrite Example",
        Value = new Microsoft.OpenApi.Any.OpenApiObject
        {
            ["Date"] = new Microsoft.OpenApi.Any.OpenApiString("2023-01-01"),
            ["Summary"] = new Microsoft.OpenApi.Any.OpenApiString("Stormy")
        }
    };
}

