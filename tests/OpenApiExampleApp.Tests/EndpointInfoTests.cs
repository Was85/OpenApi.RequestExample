using FluentAssertions;

namespace OpenApiExampleApp.Tests;

public class EndpointInfoTests
{
    [Fact]
    public void EndpointInfo_ShouldHavePathProperty()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();
        var expectedPath = "/api/test";

        // Act
        endpointInfo.Path = expectedPath;

        // Assert
        endpointInfo.Path.Should().Be(expectedPath);
    }

    [Fact]
    public void EndpointInfo_ShouldHaveOperationTypeProperty()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();
        var expectedOperationType = "Post";

        // Act
        endpointInfo.OperationType = expectedOperationType;

        // Assert
        endpointInfo.OperationType.Should().Be(expectedOperationType);
    }

    [Fact]
    public void EndpointInfo_ShouldHaveExampleNameProperty()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();
        var expectedName = "TestExample";

        // Act
        endpointInfo.ExampleName = expectedName;

        // Assert
        endpointInfo.ExampleName.Should().Be(expectedName);
    }

    [Fact]
    public void EndpointInfo_ShouldHaveExampleTypeProperty()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();
        var expectedType = "TestNamespace.TestClass";

        // Act
        endpointInfo.ExampleType = expectedType;

        // Assert
        endpointInfo.ExampleType.Should().Be(expectedType);
    }

    [Fact]
    public void EndpointInfo_ShouldHaveExampleProviderPropertyProperty()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();
        var expectedProperty = "Examples";

        // Act
        endpointInfo.ExampleProviderProperty = expectedProperty;

        // Assert
        endpointInfo.ExampleProviderProperty.Should().Be(expectedProperty);
    }

    [Fact]
    public void EndpointInfo_ShouldHaveOverwriteExistingProperty()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();
        var expectedValue = true;

        // Act
        endpointInfo.OverwriteExisting = expectedValue;

        // Assert
        endpointInfo.OverwriteExisting.Should().Be(expectedValue);
    }

    [Fact]
    public void EndpointInfo_ShouldAllowNullSymbol()
    {
        // Arrange
        var endpointInfo = new EndpointInfo();

        // Act
        endpointInfo.Symbol = null;

        // Assert
        endpointInfo.Symbol.Should().BeNull();
    }

    [Fact]
    public void EndpointInfo_ShouldStoreAllProperties()
    {
        // Arrange & Act
        var endpointInfo = new EndpointInfo
        {
            Path = "/api/weather",
            OperationType = "Get",
            ExampleName = "SunnyExample",
            ExampleType = "WeatherExamples",
            ExampleProviderProperty = "Example",
            OverwriteExisting = false,
            Symbol = null
        };

        // Assert
        endpointInfo.Path.Should().Be("/api/weather");
        endpointInfo.OperationType.Should().Be("Get");
        endpointInfo.ExampleName.Should().Be("SunnyExample");
        endpointInfo.ExampleType.Should().Be("WeatherExamples");
        endpointInfo.ExampleProviderProperty.Should().Be("Example");
        endpointInfo.OverwriteExisting.Should().BeFalse();
        endpointInfo.Symbol.Should().BeNull();
    }
}
