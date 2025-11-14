using FluentAssertions;
using OpenApiExampleApp.Attributes;

namespace OpenApiExampleApp.Tests;

public class RequestExampleAttributeTests
{
    [Fact]
    public void Constructor_ShouldSetExampleProviderType()
    {
        // Arrange
        var expectedType = typeof(string);

        // Act
        var attribute = new RequestExampleAttribute(expectedType);

        // Assert
        attribute.ExampleProviderType.Should().Be(expectedType);
    }

    [Fact]
    public void Constructor_ShouldUseDefaultValues_WhenNotProvided()
    {
        // Arrange
        var exampleType = typeof(string);

        // Act
        var attribute = new RequestExampleAttribute(exampleType);

        // Assert
        attribute.ExampleProviderProperty.Should().Be("Example");
        attribute.Name.Should().Be("Default");
    }

    [Fact]
    public void Constructor_ShouldSetExampleProviderProperty_WhenProvided()
    {
        // Arrange
        var exampleType = typeof(string);
        var expectedProperty = "CustomProperty";

        // Act
        var attribute = new RequestExampleAttribute(exampleType, exampleProviderProperty: expectedProperty);

        // Assert
        attribute.ExampleProviderProperty.Should().Be(expectedProperty);
    }

    [Fact]
    public void Constructor_ShouldSetName_WhenProvided()
    {
        // Arrange
        var exampleType = typeof(string);
        var expectedName = "CustomName";

        // Act
        var attribute = new RequestExampleAttribute(exampleType, name: expectedName);

        // Assert
        attribute.Name.Should().Be(expectedName);
    }

    [Fact]
    public void Constructor_ShouldSetAllParameters_WhenProvided()
    {
        // Arrange
        var exampleType = typeof(string);
        var expectedProperty = "CustomProperty";
        var expectedName = "CustomName";
        var expectedOverwrite = true;

        // Act
        var attribute = new RequestExampleAttribute(
            exampleType,
            exampleProviderProperty: expectedProperty,
            name: expectedName,
            overwriteExisting: expectedOverwrite);

        // Assert
        attribute.ExampleProviderType.Should().Be(exampleType);
        attribute.ExampleProviderProperty.Should().Be(expectedProperty);
        attribute.Name.Should().Be(expectedName);
    }

    [Fact]
    public void Attribute_ShouldAllowMultipleInstances()
    {
        // Arrange
        var attributeType = typeof(RequestExampleAttribute);

        // Act
        var attributeUsage = (AttributeUsageAttribute?)Attribute.GetCustomAttribute(
            attributeType,
            typeof(AttributeUsageAttribute));

        // Assert
        attributeUsage.Should().NotBeNull();
        attributeUsage!.AllowMultiple.Should().BeTrue();
    }

    [Fact]
    public void Attribute_ShouldTargetMethodAndClass()
    {
        // Arrange
        var attributeType = typeof(RequestExampleAttribute);

        // Act
        var attributeUsage = (AttributeUsageAttribute?)Attribute.GetCustomAttribute(
            attributeType,
            typeof(AttributeUsageAttribute));

        // Assert
        attributeUsage.Should().NotBeNull();
        attributeUsage!.ValidOn.Should().HaveFlag(AttributeTargets.Method);
        attributeUsage!.ValidOn.Should().HaveFlag(AttributeTargets.Class);
    }

    [Fact]
    public void Attribute_ShouldNotBeInherited()
    {
        // Arrange
        var attributeType = typeof(RequestExampleAttribute);

        // Act
        var attributeUsage = (AttributeUsageAttribute?)Attribute.GetCustomAttribute(
            attributeType,
            typeof(AttributeUsageAttribute));

        // Assert
        attributeUsage.Should().NotBeNull();
        attributeUsage!.Inherited.Should().BeFalse();
    }
}
