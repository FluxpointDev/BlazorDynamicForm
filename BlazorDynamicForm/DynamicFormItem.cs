using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace BlazorDynamicForm;

public class DynamicFormItem
{
    public DynamicFormItem(object model, PropertyInfo prop)
    {
        if (prop.PropertyType == typeof(string))
        {
            ValueType = DynamicValueType.String;
            if (prop.GetCustomAttribute<FieldTextAreaAttribute>() != null)
                FieldType = FieldType | DynamicFieldType.TextArea;
            else if (prop.GetCustomAttribute<ColorAttribute>() != null)
                FieldType = FieldType | DynamicFieldType.Color;
            else if (prop.GetCustomAttribute<PasswordPropertyTextAttribute>() != null)
                FieldType = FieldType | DynamicFieldType.Password;
            else if (prop.GetCustomAttribute<UrlAttribute>() != null)
                FieldType = FieldType | DynamicFieldType.Url;
            else if (prop.GetCustomAttribute<PhoneAttribute>() != null)
                FieldType= FieldType | DynamicFieldType.Phone;

        }

        // Temp removed due to issues.
        //else if (prop.PropertyType == typeof(DateTime))
        //{
        //    ValueType = DynamicValueType.Date;
        //    FieldType = DynamicFieldType.Date;
        //}
        else if (prop.PropertyType == typeof(int))
        {
            ValueType = DynamicValueType.Int;
            FieldType = DynamicFieldType.Number;
        }
        else if (prop.PropertyType == typeof(long))
        {
            ValueType = DynamicValueType.Long;
            FieldType = DynamicFieldType.Number;
        }
        else if (prop.PropertyType == typeof(short))
        {
            ValueType = DynamicValueType.Short;
            FieldType = DynamicFieldType.Number;
        }
        else if (prop.PropertyType == typeof(double))
        {
            ValueType = DynamicValueType.Double;
            FieldType = DynamicFieldType.Number;
        }
        else if (prop.PropertyType == typeof(ulong))
        {
            ValueType = DynamicValueType.Ulong;
            FieldType = DynamicFieldType.Number;
            
        }
        else if (prop.PropertyType == typeof(bool))
        {
            ValueType = DynamicValueType.Bool;
            FieldType = DynamicFieldType.Checkbox;
        }
        else
            throw new ArgumentException("Invalid property type for " + prop.Name);

        IsHidden = prop.GetCustomAttribute<HiddenAttribute>() != null;

        if (FieldType.HasFlag(DynamicFieldType.Number))
        {
            IsSlider = prop.GetCustomAttribute<RangeSliderAttribute>() != null;
            Range = prop.GetCustomAttribute<RangeAttribute>();
            RangeStep = prop.GetCustomAttribute<RangeStepAttribute>();
        }

        SelectOptionAttribute? Select = prop.GetCustomAttribute<SelectOptionAttribute>();
        if (Select != null)
            FieldType = DynamicFieldType.Select;

        SetLabelText(prop);
        DescriptionAttribute? Desc = prop.GetCustomAttribute<DescriptionAttribute>();
        if (Desc != null && !string.IsNullOrEmpty(Desc.Description))
            Description = Desc.Description;

        Property = prop;
    }

    internal RenderFragment RenderField { get; set; }

    internal DynamicValueType ValueType { get; set; }
    internal DynamicFieldType FieldType { get; set; }

    private string SetLabelText(PropertyInfo prop)
    {
        bool HasDisplayName = false;
        
        DisplayAttribute? DisplayName = prop.GetCustomAttribute<DisplayAttribute>();
        if (DisplayName != null && !string.IsNullOrEmpty(DisplayName.Name))
        {
            Label = DisplayName.Name;
            HasDisplayName = true;
        }
        else
        {
            DisplayNameAttribute DisplayName2 = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (DisplayName2 != null && !string.IsNullOrEmpty(DisplayName2.DisplayName))
            {
                Label = DisplayName2.DisplayName;
                HasDisplayName = true;
            }
        }

        if (!HasDisplayName)
        {
            bool IsUppercase = false;
            StringBuilder str = new StringBuilder();
            foreach (char i in prop.Name)
            {
                if (char.IsUpper(i))
                {
                    if (!IsUppercase)
                    {
                        str.Append(' ');
                        IsUppercase = true;
                    }

                }
                else
                {
                    if (IsUppercase)
                    {
                        IsUppercase = false;
                    }
                }
                str.Append(i);
            }
            Label = str.ToString();
        }
        return Label;
    }

    public PropertyInfo Property { get; private set; }

    public string Label { get; private set; }

    public string Description { get; private set; }

    public string ErrorMessage { get; internal set; }

    internal bool IsInvalidValue { get; set; }

    public bool IsHidden { get; internal set; }

    public bool IsSlider { get; internal set; }

    public RangeAttribute? Range { get; internal set; }

    public RangeStepAttribute? RangeStep { get; internal set; }
    internal bool Validate(object model)
    {
        ValidationContext Context = new ValidationContext(Property.GetType());
        foreach (ValidationAttribute i in Property.GetCustomAttributes<ValidationAttribute>())
        {
            object? Value = Property.GetValue(model);
            ValidationResult? Validate = i.GetValidationResult(Value, Context);
            if (Validate != null && !string.IsNullOrEmpty(Validate.ErrorMessage))
            {
                ErrorMessage = Validate.ErrorMessage;
                return false;
            }

            if (i is EmailAddressAttribute emailAttribute)
            {
                string Email = (string)Value;
                
                int indexDot = Email.IndexOf('.');
                if (indexDot == 0 || Email.EndsWith('.'))
                {
                    ErrorMessage = emailAttribute.FormatErrorMessage(Label);
                    return false;
                }
                int indexAt = Email.IndexOf('@');
                if (indexDot < indexAt)
                {
                    ErrorMessage = emailAttribute.FormatErrorMessage(Label);
                    return false;
                }
            }
        }

        return true;
    }
}
