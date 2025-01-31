namespace WebApi_OpenApiExampleDemo.Controllers
{
    public static class LogEventExamples
    {

        public static Microsoft.OpenApi.Models.OpenApiExample Example => new Microsoft.OpenApi.Models.OpenApiExample
        {
            Summary = "Example Log Event",
            Description = "This is a sample log event.",
            Value = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["LogAgentValueId"] = new Microsoft.OpenApi.Any.OpenApiInteger(42),
                ["LogText"] = new Microsoft.OpenApi.Any.OpenApiString("File OK")
            }
        };
    }
}


