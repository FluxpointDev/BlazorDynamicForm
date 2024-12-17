namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FileCameraAttribute : Attribute
{
    public FileCameraAttribute(FormFileCameraType type)
    {
        Type = type;
    }

    public FormFileCameraType Type { get; private set; }
}
public enum FormFileCameraType
{
    Front, Back
}

