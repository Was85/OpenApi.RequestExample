using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class SyntaxReceiver : ISyntaxContextReceiver
{
    public List<EndpointInfo> Endpoints { get; } = new List<EndpointInfo>();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        var methodDeclaration = context.Node as MethodDeclarationSyntax;
        if (methodDeclaration == null)
        {
            return;
        }

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration) as IMethodSymbol;
        if (methodSymbol == null) return;

        var attributeData = methodSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.Name == "RequestExampleAttribute");

        if (attributeData == null) return;

        var exampleType = attributeData.ConstructorArguments[0].Value?.ToString();
        var exampleProviderProperty = attributeData.ConstructorArguments[1].Value?.ToString();
        var exampleName = attributeData.ConstructorArguments.Length > 2
            ? attributeData.ConstructorArguments[2].Value?.ToString() ?? "Default"
            : "Default";

        var path = ExtractRoute(methodSymbol);
        Debug.WriteLine($"[RequestExample] Found: {methodSymbol.Name}, Path: {path}, Example: {exampleType}.{exampleProviderProperty}");
        var endpoint = new EndpointInfo
        {
            Path = path,
            OperationType = GetOperationType(methodSymbol),
            ExampleName = exampleName,
            ExampleType = exampleType,
            ExampleProviderProperty = exampleProviderProperty,
            Symbol = methodSymbol
        };
        Endpoints.Add(endpoint);
    }

    private string RemoveControllerSuffix(string typeName)
    {
        const string controllerSuffix = "Controller";
        if (typeName.EndsWith(controllerSuffix))
        {
            return typeName.Substring(0, typeName.Length - controllerSuffix.Length);
        }
        return typeName;
    }

    private string GetOperationType(IMethodSymbol methodSymbol)
    {
        var attributes = methodSymbol.GetAttributes();
        if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpGetAttribute")) return "Get";
        if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpPostAttribute")) return "Post";
        if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpPutAttribute")) return "Put";
        if (attributes.Any(attr => attr.AttributeClass?.Name == "HttpDeleteAttribute")) return "Delete";

        return "Post";
    }

    private string ExtractRoute(IMethodSymbol methodSymbol)
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