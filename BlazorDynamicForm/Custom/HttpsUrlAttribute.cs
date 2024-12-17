using System.Globalization;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class HttpsUrlAttribute : ValidationAttribute
{
    public HttpsUrlAttribute()
    {
        if (string.IsNullOrEmpty(ErrorMessage))
            ErrorMessage = $"{0} requires a secure https url.";
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not string str)
            return false;

        return str.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(CultureInfo.CurrentCulture,
          ErrorMessageString, name);
    }
}
