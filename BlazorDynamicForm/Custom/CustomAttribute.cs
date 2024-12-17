namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class CustomAttribute : Attribute
{
    public CustomAttribute(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; private set; }
    public string? Value { get; private set; }
}
