using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace OpenApiExampleApp.SourceGenerators
{
    [Generator]
    public class OpenApiExampleSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register a syntax receiver that finds attribute usages
            var syntaxProvider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (s, _) => IsMethodWithRequestExampleAttribute(s),
                    transform: (ctx, _) => GetEndpointInfo(ctx)).Where(endpoint => endpoint != null);

            var compilationAndEndpoints = context.CompilationProvider.Combine(syntaxProvider.Collect());

            context.RegisterSourceOutput(compilationAndEndpoints, (spc, source) => Execute(source.Left, source.Right, spc));
        }

        private static bool IsMethodWithRequestExampleAttribute(SyntaxNode node)
        {
            bool hasRequestExampleAttribute = node is MethodDeclarationSyntax methodDeclaration &&
                   methodDeclaration.AttributeLists
                       .SelectMany(al => al.Attributes)
                       .Any(attr => attr.Name.ToString().Contains("RequestExample"));

            return hasRequestExampleAttribute;
        }

        private static EndpointInfo GetEndpointInfo(GeneratorSyntaxContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;
            var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration) as IMethodSymbol;
        
            if (methodSymbol == null)
            {
                return null;
            }

            var attributeData = methodSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "RequestExampleAttribute");

            if (attributeData == null)
            {
                return null;
            }

            var exampleType = attributeData.ConstructorArguments[0].Value?.ToString();
            var exampleProviderProperty = attributeData.ConstructorArguments[1].Value?.ToString();
            var exampleName = attributeData.ConstructorArguments.Length > 2 ? attributeData.ConstructorArguments[2].Value?.ToString() ?? "Default" : "Default";

            var endPointInfo = new EndpointInfo
            {
                Path = ExtractRoute(methodSymbol),
                OperationType = GetOperationType(methodSymbol),
                ExampleName = exampleName,
                ExampleType = exampleType,
                ExampleProviderProperty = exampleProviderProperty,
                Symbol = methodSymbol
            };

            return endPointInfo;
        }

        private static void Execute(Compilation compilation, ImmutableArray<EndpointInfo> endpoints, SourceProductionContext context)
        {
            var source = GenerateOpenApiTransformer(context, compilation, endpoints);
            context.AddSource("GeneratedOpenApiDocumentTransformer", SourceText.From(source, Encoding.UTF8));
        }

        private static string GenerateOpenApiTransformer(SourceProductionContext context,Compilation compilation, ImmutableArray<EndpointInfo> endpoints)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using Microsoft.OpenApi.Models;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Microsoft.AspNetCore.OpenApi;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Threading.Tasks;");

            sb.AppendLine("public class GeneratedOpenApiDocumentTransformer : IOpenApiDocumentTransformer");
            sb.AppendLine("{");
            sb.AppendLine("    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)");
            sb.AppendLine("    {");

            foreach (var endpoint in endpoints)
            {
                if (!HasStaticProperty(endpoint.ExampleType, endpoint.ExampleProviderProperty, compilation))
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

        private static bool HasStaticProperty(string typeName, string propertyName, Compilation compilation)
        {
            var type = compilation.GetTypeByMetadataName(typeName);
            return type?.GetMembers(propertyName).FirstOrDefault() is IPropertySymbol property && property.IsStatic;
        }

        private static void ReportDiagnostic(SourceProductionContext context, ISymbol symbol, string message, string title, string category)
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

        private static string RemoveControllerSuffix(string typeName)
        {
            const string controllerSuffix = "Controller";
            if (typeName.EndsWith(controllerSuffix))
            {
                return typeName.Substring(0, typeName.Length - controllerSuffix.Length);
            }
            return typeName;
        }

        private static string GetOperationType(IMethodSymbol methodSymbol)
        {
            var attributes = methodSymbol.GetAttributes();
            if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpGetAttribute")) return "Get";
            if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpPostAttribute")) return "Post";
            if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpPutAttribute")) return "Put";
            if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpDeleteAttribute")) return "Delete";

            return "Post";
        }

        private static string ExtractRoute(IMethodSymbol methodSymbol)
        {
            var controllerSymbol = methodSymbol.ContainingType;

            // Get controller-level route if present
            var controllerRoute = controllerSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "RouteAttribute")
                ?.ConstructorArguments.FirstOrDefault().Value?.ToString();

            // Handle token replacement (e.g., [controller])
            if (!string.IsNullOrWhiteSpace(controllerRoute))
            {
                controllerRoute = controllerRoute.Replace("[controller]", RemoveControllerSuffix(controllerSymbol.Name));
            }

            // Get method-level route if present
            var methodRouteAttribute = methodSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "HttpGetAttribute" ||
                                        attr.AttributeClass?.Name == "HttpPostAttribute" ||
                                        attr.AttributeClass?.Name == "HttpPutAttribute" ||
                                        attr.AttributeClass?.Name == "HttpDeleteAttribute");

            var methodRoute = methodRouteAttribute?.ConstructorArguments.FirstOrDefault().Value?.ToString();

            // Construct full route: Controller Route + Method Route
            if (!string.IsNullOrWhiteSpace(controllerRoute) && !string.IsNullOrWhiteSpace(methodRoute))
            {
                return $"/{controllerRoute.TrimEnd('/')}/{methodRoute.TrimStart('/')}";
            }

            if (!string.IsNullOrWhiteSpace(controllerRoute))
            {
                return $"/{controllerRoute.TrimEnd('/')}";
            }

            if (!string.IsNullOrWhiteSpace(methodRoute))
            {
                return $"/{RemoveControllerSuffix(controllerSymbol.Name)}/{methodRoute.TrimStart('/')}";
            }

            // Fallback: Infer from controller name and method name
            return $"/{RemoveControllerSuffix(controllerSymbol.Name)}/{methodSymbol.Name}";
        }
    }
}
