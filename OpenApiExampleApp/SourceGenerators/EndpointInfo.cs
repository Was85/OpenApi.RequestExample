using Microsoft.CodeAnalysis;

public class EndpointInfo
{
    /// <summary>
    /// The route path for the endpoint (e.g., "/LogAPI/api/LogEvent").
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The HTTP operation type (e.g., "Get", "Post", "Put", "Delete").
    /// </summary>
    public string OperationType { get; set; }

    /// <summary>
    /// The name of the example in the OpenAPI Examples dictionary (e.g., "LogEvent Example").
    /// </summary>
    public string ExampleName { get; set; }

    /// <summary>
    /// The fully qualified type name of the example provider class (e.g., "Namespace.LogEventExamples").
    /// </summary>
    public string ExampleType { get; set; }

    /// <summary>
    /// The name of the static property on the example provider class (e.g., "Example").
    /// </summary>
    public string ExampleProviderProperty { get; set; }

    /// <summary>
    /// The method symbol representing the endpoint. Used for diagnostics.
    /// </summary>
    public ISymbol Symbol { get; set; }
}
