using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<GeneratedOpenApiTransformers>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Demo");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public class GeneratedOpenApiTransformers : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // Adding example for /WeatherForecast (Get)
        if (document.Paths.ContainsKey("/WeatherForecast"))
        {
            var operation = document.Paths["/WeatherForecast"].Operations[OperationType.Get];
            if (operation.RequestBody?.Content?.ContainsKey("application/json") == true)
            {
                if (operation.RequestBody.Content["application/json"].Examples == null)
                {
                    operation.RequestBody.Content["application/json"].Examples = new Dictionary<string, OpenApiExample>();
                }
                operation.RequestBody.Content["application/json"].Examples["WeatherForecast Examples"] = WebApi_OpenApiExampleDemo.Controllers.WeatherForecastExamples.Example;
            }
        }
        // Adding example for /WeatherForecast/test (Get)
        if (document.Paths.ContainsKey("/WeatherForecast/test"))
        {
            var operation = document.Paths["/WeatherForecast/test"].Operations[OperationType.Get];
            if (operation.RequestBody?.Content?.ContainsKey("application/json") == true)
            {
                if (operation.RequestBody.Content["application/json"].Examples == null)
                {
                    operation.RequestBody.Content["application/json"].Examples = new Dictionary<string, OpenApiExample>();
                }
                operation.RequestBody.Content["application/json"].Examples["WeatherForecast Examples"] = WebApi_OpenApiExampleDemo.Controllers.WeatherForecastExamples.Example;
            }
        }
        return Task.CompletedTask;
    }
}