using System.Globalization;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class RangeStepAttribute : ValidationAttribute
{
    public RangeStepAttribute(int value)
    {
        Value = value;
        if (string.IsNullOrEmpty(ErrorMessage))
            ErrorMessage = $"{0} number is outside the allowed steps of {Value}.";
    }

    public RangeStepAttribute(double value)
    {
        Value = value;
        if (string.IsNullOrEmpty(ErrorMessage))
            ErrorMessage = $"{0} number is outside the allowed steps {Value}.";
    }

    public object Value { get; private set; }


    public override bool IsValid(object value)
    {
        bool Valid = false;
        
        if (value is int number)
            Valid = number % (int)Value == 0;
        else if (value is double dble)
            Valid = dble % (double)Value == 0;
        else
            Valid = true;

        return Valid;
    }

    public override string FormatErrorMessage(string name)
    {
        return String.Format(CultureInfo.CurrentCulture,
          ErrorMessageString, name, Value);
    }
}
