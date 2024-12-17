namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class HintsAttribute : Attribute
{
    public HintsAttribute(string[] options)
    {
        Options = options;
    }

    public string[] Options { get; private set; }

}