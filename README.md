# OpenAPI Request Example Generator

**OpenAPI Request Example Generator** is a NuGet package designed for .NET 9 and above to simplify adding request examples to your API documentation in **Swagger UI**, **ReDoc UI** or **Scalar UI**. By using simple attributes on your API controller actions, the package automatically generates an OpenAPI transform document, enriching your API docs with meaningful examples. It is currently limited to standard API controllers and does not support Minimal APIs

## Features

- **Seamless Integration** with Swagger UI, ReDoc and Scalar UI.
- **Simple Attribute-Based Configuration** – just add attributes to your API actions.
- **Automatic OpenAPI Transform Document Generation**.
- **Minimal Setup** – install, annotate, and configure your OpenAPI services.

## Installation

Install the NuGet package via the .NET CLI:

```sh
dotnet add package OpenApiRequestExample
```

Or via the Package Manager Console:

```powershell
Install-Package OpenApiRequestExample
```

## Getting Started

### 1. Annotate Your API Actions

Add the `[RequestExample]` attribute to your API controller actions to specify request examples.

```csharp

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpPost]
    [RequestExample(typeof(WeatherForecastExample))]
    public IActionResult CreateItem(WeatherForecastDto weatherForecastDto)
    {
        return Ok();
    }
}
```
### 2. Providing Example Data via Static Class  
To define reusable examples, create a static class with a dictionary of named examples:

```csharp
public static class WeatherForecastExample
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
```

### 3. Attribute Options  
The `[RequestExample]` attribute provides additional options for customization:

```csharp
public RequestExampleAttribute(
    Type exampleProviderType,
    string exampleProviderProperty = "Example",
    string name = "Default",
    bool overwriteExisting = false)
{
    OverwriteExisting = overwriteExisting;
    ExampleProviderType = exampleProviderType;
    ExampleProviderProperty = exampleProviderProperty;
    Name = name;
}
```

- `exampleProviderType` (**Required**): Specifies the type of the class that provides the example.
- `exampleProviderProperty` (*Default: "Example"*): The property name within the provider class that holds the example value.
- `name` (*Default: "Default"*): A custom name for the example, useful for differentiating multiple examples.
- `overwriteExisting` (*Default: false*): Determines if existing examples should be overwritten.

To specify an example with a custom name:

```csharp
[RequestExample(typeof(WeatherForecastExample), name: "MyExample")]
```

To force an override of an existing example:

```csharp
[RequestExample(typeof(WeatherForecastExample), overwriteExisting: true)]
```
### 4. Configure OpenAPI Services

Add the transform document generation to your OpenAPI configuration in `Program.cs`.

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<GeneratedOpenApiDocumentTransformer>();
});
```

### 3. Run Your Application

Start your application and navigate to the Swagger UI or Scalar UI. You'll see the request examples displayed for your API endpoints.

## Example Output

After adding the attribute and configuring the services, your Swagger UI or Scalar UI will display request examples like this:

```json
{
  "Date": "2021-07-01",
  "Summary": "Sunny"
}
```

```json
{
  "Date": "2021-07-02",
  "Summary": "Rainy"
}
```


## Customization

You can customize the request examples by:

- Using different models with the `[RequestExample]` attribute.
- Specifying complex JSON structures for advanced request bodies.


## License

This project is licensed under the MIT License.

