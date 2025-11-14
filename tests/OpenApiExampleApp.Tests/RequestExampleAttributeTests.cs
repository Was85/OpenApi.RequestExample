using OpenApiExampleApp.Attributes;

namespace OpenApiExampleApp.Tests;

public class RequestExampleAttributeTests
{
    [Fact]
    public void Constructor_WithRequiredParameter_ShouldSetProperties()
    {
        // Arrange
        var exampleType = typeof(string);

        // Act
        var attribute = new RequestExampleAttribute(exampleType);

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(exampleType, attribute.ExampleProviderType);
        Assert.Equal("Example", attribute.ExampleProviderProperty);
        Assert.Equal("Default", attribute.Name);
    }

    [Fact]
    public void Constructor_WithAllParameters_ShouldSetProperties()
    {
        // Arrange
        var exampleType = typeof(string);
        var exampleProviderProperty = "CustomProperty";
        var name = "CustomName";
        var overwriteExisting = true;

        // Act
        var attribute = new RequestExampleAttribute(
            exampleType,
            exampleProviderProperty,
            name,
            overwriteExisting);

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(exampleType, attribute.ExampleProviderType);
        Assert.Equal(exampleProviderProperty, attribute.ExampleProviderProperty);
        Assert.Equal(name, attribute.Name);
    }

    [Fact]
    public void Constructor_WithNullExampleProviderType_ShouldNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => new RequestExampleAttribute(null));
        Assert.Null(exception);
    }

    [Fact]
    public void Attribute_CanBeAppliedToMethod()
    {
        // Arrange
        var methodInfo = typeof(TestController).GetMethod(nameof(TestController.TestMethod));

        // Act
        var attributes = methodInfo?.GetCustomAttributes(typeof(RequestExampleAttribute), false);

        // Assert
        Assert.NotNull(attributes);
        Assert.Single(attributes);
        var attribute = attributes[0] as RequestExampleAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(typeof(TestExampleProvider), attribute.ExampleProviderType);
    }

    [Fact]
    public void Attribute_AllowsMultipleInstances()
    {
        // Arrange
        var methodInfo = typeof(TestController).GetMethod(nameof(TestController.MultipleAttributesMethod));

        // Act
        var attributes = methodInfo?.GetCustomAttributes(typeof(RequestExampleAttribute), false);

        // Assert
        Assert.NotNull(attributes);
        Assert.Equal(2, attributes.Length);
    }

    // Test classes
    private class TestController
    {
        [RequestExample(typeof(TestExampleProvider))]
        public void TestMethod() { }

        [RequestExample(typeof(TestExampleProvider), name: "Example1")]
        [RequestExample(typeof(TestExampleProvider), name: "Example2")]
        public void MultipleAttributesMethod() { }
    }

    private class TestExampleProvider { }
}
