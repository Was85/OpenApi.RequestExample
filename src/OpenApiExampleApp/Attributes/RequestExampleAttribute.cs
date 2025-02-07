namespace OpenApiExampleApp.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class RequestExampleAttribute : Attribute
    {
        private bool OverwriteExisting;
        public Type ExampleProviderType { get; }
        public string ExampleProviderProperty { get; }
        public string Name { get; }
        
        public RequestExampleAttribute(
            Type exampleProviderType,
            string exampleProviderProperty ="Example",
            string name = "Default",
            bool overwriteExisting = false)
        {
            OverwriteExisting = overwriteExisting;
            ExampleProviderType = exampleProviderType;
            ExampleProviderProperty = exampleProviderProperty;
            Name = name;
        }
    }

}

