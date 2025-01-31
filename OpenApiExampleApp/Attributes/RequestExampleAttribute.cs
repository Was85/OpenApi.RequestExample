namespace OpenApiExampleApp.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class RequestExampleAttribute : Attribute
    {
        public Type ExampleProviderType { get; }
        public string ExampleProviderProperty { get; }
        public string Name { get; }

        public RequestExampleAttribute(Type exampleProviderType, string exampleProviderProperty, string name = "Default")
        {
            ExampleProviderType = exampleProviderType;
            ExampleProviderProperty = exampleProviderProperty;
            Name = name;
        }
    }

}

