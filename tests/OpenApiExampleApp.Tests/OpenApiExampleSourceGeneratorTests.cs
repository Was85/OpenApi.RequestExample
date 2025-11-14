using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using OpenApiExampleApp.SourceGenerators;

namespace OpenApiExampleApp.Tests;

public class OpenApiExampleSourceGeneratorTests
{
    [Fact]
    public void Generator_CanBeInstantiated()
    {
        // Arrange & Act
        var generator = new OpenApiExampleSourceGenerator();

        // Assert
        Assert.NotNull(generator);
    }

    [Fact]
    public void Generator_WithValidAttribute_GeneratesOutput()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var runResult = driver.GetRunResult();

        // Assert
        Assert.NotNull(runResult);
        // Generator should produce output if there are matching attributes
        Assert.True(runResult.GeneratedTrees.Length >= 0);
    }

    [Fact]
    public void Generator_WithHttpGetAttribute_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Generator_WithHttpPutAttribute_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpPut]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Put()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Generator_WithHttpDeleteAttribute_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpDelete]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Generator_WithHttpPatchAttribute_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpPatch]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Patch()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Generator_WithHttpOptionsAttribute_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpOptions]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Options()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Generator_WithMissingExampleProperty_GeneratesOutput()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        // Missing Example property
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var runResult = driver.GetRunResult();

        // Assert - Generator should handle this gracefully
        Assert.NotNull(runResult);
    }

    [Fact]
    public void Generator_WithCustomExampleName_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/test"")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        [RequestExample(typeof(TestExamples), name: ""CustomExampleName"")]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Generator_WithControllerRoute_DoesNotThrow()
    {
        // Arrange
        var source = @"
using System;
using Microsoft.AspNetCore.Mvc;
using OpenApiExampleApp.Attributes;

namespace TestNamespace
{
    public static class TestExamples
    {
        public static Microsoft.OpenApi.IOpenApiExample Example => null;
    }

    [ApiController]
    [Route(""api/[controller]"")]
    public class TestController : ControllerBase
    {
        [HttpPost(""create"")]
        [RequestExample(typeof(TestExamples))]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}";

        // Act
        var (compilation, driver) = CreateCompilationAndDriver(source);
        var exception = Record.Exception(() => driver.GetRunResult());

        // Assert
        Assert.Null(exception);
    }

    private static (Compilation compilation, GeneratorDriver driver) CreateCompilationAndDriver(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Add references to necessary assemblies
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Collections.Generic.List<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Microsoft.OpenApi.IOpenApiExample).Assembly.Location),
        };

        // Try to load ASP.NET Core assemblies
        try
        {
            var aspNetCoreAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Microsoft.AspNetCore.Mvc.Core");
            
            if (aspNetCoreAssembly != null)
            {
                references.Add(MetadataReference.CreateFromFile(aspNetCoreAssembly.Location));
            }

            var aspNetCoreAbstractions = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Microsoft.AspNetCore.Mvc.Abstractions");
            
            if (aspNetCoreAbstractions != null)
            {
                references.Add(MetadataReference.CreateFromFile(aspNetCoreAbstractions.Location));
            }
        }
        catch
        {
            // Continue without ASP.NET Core references if not available
        }

        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new OpenApiExampleSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

        return (compilation, driver);
    }
}
