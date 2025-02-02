namespace DemoApi.RequestExamples;

public static class TemperatureExamples
{
    public static Microsoft.OpenApi.Models.OpenApiExample Example => new Microsoft.OpenApi.Models.OpenApiExample
    {
        Summary = "Temperature Example",
        Value = new Microsoft.OpenApi.Any.OpenApiObject
        {
            ["Celsius"] = new Microsoft.OpenApi.Any.OpenApiInteger(25),
            ["Fahrenheit"] = new Microsoft.OpenApi.Any.OpenApiInteger(77)
        }
    };
}