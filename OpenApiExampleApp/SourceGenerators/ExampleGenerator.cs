using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using System.Net;
using System.Linq;
using System.Diagnostics;



[Generator]
public class OpenApiExampleSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {

        // Register a syntax receiver that finds attribute usages
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Retrieve the syntax receiver
        var receiver = context.SyntaxContextReceiver as SyntaxReceiver;
        if (receiver == null)
        {
            return;
        }
        Debug.WriteLine($"Found {receiver.Endpoints.Count} endpoints");

        var source = GenerateOpenApiTransformer(context, receiver.Endpoints);
        context.AddSource("GeneratedOpenApiTransformer", SourceText.From(source, Encoding.UTF8));
    }


    private string GenerateOpenApiTransformer(GeneratorExecutionContext context, List<EndpointInfo> endpoints)
    {
        var sb = new StringBuilder();

        sb.AppendLine("using Microsoft.OpenApi.Models;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using Microsoft.AspNetCore.OpenApi;");
        sb.AppendLine("using System.Threading;");
        sb.AppendLine("using System.Threading.Tasks;");

        sb.AppendLine("public class GeneratedOpenApiTransformer : IOpenApiDocumentTransformer");
        sb.AppendLine("{");
        sb.AppendLine("    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)");
        sb.AppendLine("    {");

        foreach (var endpoint in endpoints)
        {
            if (!HasStaticProperty(endpoint.ExampleType, endpoint.ExampleProviderProperty, context.Compilation))
            {
                ReportDiagnostic(
                    context,
                    endpoint.Symbol,
                    $"The example provider '{endpoint.ExampleProviderProperty}' does not exist on the type '{endpoint.ExampleType}'.",
                    "Example Not Found",
                    "OpenApiExample"
                );
                continue;
            }

            sb.AppendLine($"        // Adding example for {endpoint.Path} ({endpoint.OperationType})");
            sb.AppendLine($"        if (document.Paths.ContainsKey(\"{endpoint.Path}\"))");
            sb.AppendLine("        {");
            sb.AppendLine($"            var operation = document.Paths[\"{endpoint.Path}\"].Operations[OperationType.{endpoint.OperationType}];");
            sb.AppendLine($"            if (operation.RequestBody?.Content?.ContainsKey(\"application/json\") == true)");
            sb.AppendLine("            {");
            sb.AppendLine($"                if (operation.RequestBody.Content[\"application/json\"].Examples == null)");
            sb.AppendLine("                {");
            sb.AppendLine("                    operation.RequestBody.Content[\"application/json\"].Examples = new Dictionary<string, OpenApiExample>();");
            sb.AppendLine("                }");
            sb.AppendLine($"                operation.RequestBody.Content[\"application/json\"].Examples[\"{endpoint.ExampleName}\"] = {endpoint.ExampleType}.{endpoint.ExampleProviderProperty};");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
        }

        sb.AppendLine("        return Task.CompletedTask;");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    /// <summary>
    /// Checks if a static property exists on a type.
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="propertyName"></param>
    /// <param name="compilation"></param>
    /// <returns></returns>
    private bool HasStaticProperty(string typeName, string propertyName, Compilation compilation)
    {
        var type = compilation.GetTypeByMetadataName(typeName);
        return type?.GetMembers(propertyName).FirstOrDefault() is IPropertySymbol property && property.IsStatic;
    }

    private void ReportDiagnostic(GeneratorExecutionContext context, ISymbol symbol, string message, string title, string category)
    {
        var diagnostic = Diagnostic.Create(
            new DiagnosticDescriptor(
                id: "GEN001",
                title: title,
                messageFormat: message,
                category: category,
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            symbol.Locations.FirstOrDefault());

        context.ReportDiagnostic(diagnostic);
    }

}
