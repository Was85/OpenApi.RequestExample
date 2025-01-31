//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.OpenApi;
//using Microsoft.OpenApi.Models;
//using OpenApiExample.Attributes;

//namespace OpenApiExample.Transformers
//{
//    public class OpenApiExampleTransformer : IOpenApiDocumentTransformer
//    {
//        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
//        {
//            var assembly = Assembly.GetExecutingAssembly();

//            // 1. Process model-level examples
//            var modelTypes = assembly.GetTypes()
//                .Where(t => t.GetCustomAttributes<RequestExampleAttribute>().Any())
//                .ToList();

//            foreach (var modelType in modelTypes)
//            {
//                var exampleAttribute = modelType.GetCustomAttribute<RequestExampleAttribute>();
//                if (exampleAttribute != null)
//                {
//                    var example = GetExampleFromAttribute(exampleAttribute);
//                    if (example != null)
//                    {
//                        var schemaKey = document.Components.Schemas.Keys.FirstOrDefault(k => k.Equals(modelType.Name, StringComparison.OrdinalIgnoreCase));
//                        if (schemaKey != null)
//                        {
//                            document.Components.Schemas[schemaKey].Example = example.Value;
//                        }
//                    }
//                }
//            }

//            // 2. Process endpoint-level examples
//            foreach (var path in document.Paths)
//            {
//                foreach (var operation in path.Value.Operations)
//                {
//                    var methodInfo = operation.Value.Extensions.TryGetValue("x-methodInfo", out var extension)
//                        ? extension as MethodInfo
//                        : null;

//                    if (methodInfo == null)
//                    {
//                        continue;
//                    }

//                    var exampleAttributes = methodInfo.GetCustomAttributes<RequestExampleAttribute>();
//                    foreach (var exampleAttribute in exampleAttributes)
//                    {
//                        var example = GetExampleFromAttribute(exampleAttribute);
//                        if (example != null)
//                        {
//                            if (operation.Value.RequestBody?.Content?.ContainsKey("application/json") == true)
//                            {
//                                if (operation.Value.RequestBody.Content["application/json"].Examples == null)
//                                {
//                                    operation.Value.RequestBody.Content["application/json"].Examples = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiExample>();
//                                }
//                                operation.Value.RequestBody.Content["application/json"].Examples[exampleAttribute.Name] = example;
//                            }
//                        }
//                    }
//                }
//            }

//            return Task.CompletedTask;
//        }

//        private Microsoft.OpenApi.Models.OpenApiExample GetExampleFromAttribute(RequestExampleAttribute attribute)
//        {
//            var providerType = attribute.ExampleProviderType;
//            var providerProperty = providerType.GetProperty(attribute.ExampleProviderProperty, BindingFlags.Static | BindingFlags.Public);

//            if (providerProperty == null)
//            {
//                throw new InvalidOperationException($"The property '{attribute.ExampleProviderProperty}' was not found on type '{providerType.FullName}'.");
//            }

//            var example = providerProperty.GetValue(null) as Microsoft.OpenApi.Models.OpenApiExample;
//            if (example == null)
//            {
//                throw new InvalidOperationException($"The property '{attribute.ExampleProviderProperty}' must return an instance of 'OpenApiExample'.");
//            }

//            return example;
//        }
//    }
//}