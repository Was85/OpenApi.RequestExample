namespace WebApi_OpenApiExampleDemo.Controllers
{
    public static class WeatherForecastExamples
    {

        public static Microsoft.OpenApi.Models.OpenApiExample Example => new Microsoft.OpenApi.Models.OpenApiExample
        {

            Summary = "Example Weather Forecast",
            Description = "This is a sample weather forecast.",
            Value = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["Date"] = new Microsoft.OpenApi.Any.OpenApiString("2021-07-01"),
                ["TemperatureC"] = new Microsoft.OpenApi.Any.OpenApiInteger(20),
                ["TemperatureF"] = new Microsoft.OpenApi.Any.OpenApiInteger(68),
                ["Summary"] = new Microsoft.OpenApi.Any.OpenApiString("Mild")
            }
        };
    }
}


