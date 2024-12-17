using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace BlazorDynamicForm;

internal sealed class DynamicClassicTheme : IDynamicFormTheme
{
    public static DynamicClassicTheme Instance = new DynamicClassicTheme();
    public void Build<Model>(RenderTreeBuilder builder, IDynamicFormBase<Model> form, object model, DynamicFormItem item, PropertyInfo prop)
    {
        string Name = prop.Name;

        if (item.IsHidden)
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", "hidden");
            builder.AddAttribute(2, "disabled");
        }
        else
        {
            if (item.FieldType.HasFlag(DynamicFieldType.Select))
            {
                IEnumerable<SelectOptionAttribute> Select = prop.GetCustomAttributes<SelectOptionAttribute>();
                builder.OpenElement(0, "select");

                foreach (var i in Select)
                {
                    builder.OpenElement(0, "option");
                    builder.AddContent(1, i.Name);
                    //builder.AddAttribute(3, "value", i.Value);
                    builder.CloseElement();
                }
            }
            else
            {
                switch (item.ValueType)
                {
                    case DynamicValueType.String:
                        {
                            if (item.FieldType.HasFlag(DynamicFieldType.TextArea))
                                builder.OpenElement(0, "textarea");
                            else
                                builder.OpenElement(0, "input");

                            if (item.FieldType.HasFlag(DynamicFieldType.Color))
                                builder.AddAttribute(1, "type", "color");
                            else if (item.FieldType.HasFlag(DynamicFieldType.Password))
                                builder.AddAttribute(1, "type", "password");
                            else if (item.FieldType.HasFlag(DynamicFieldType.Url))
                                builder.AddAttribute(1, "type", "url");
                            else if (item.FieldType.HasFlag(DynamicFieldType.Phone))
                                builder.AddAttribute(1, "type", "tel");
                        }
                        break;
                    case DynamicValueType.Int:
                    case DynamicValueType.Long:
                    case DynamicValueType.Short:
                    case DynamicValueType.Ulong:
                    case DynamicValueType.Double:
                        {
                            builder.OpenElement(0, "input");

                            if (item.Range != null && item.IsSlider)
                                builder.AddAttribute(1, "type", "range");
                            else
                                builder.AddAttribute(1, "type", "number");
                        }
                        break;
                    case DynamicValueType.Bool:
                        {
                            builder.OpenElement(0, "input");
                            builder.AddAttribute(1, "type", "checkbox");
                        }
                        break;
                    default:
                        throw new Exception("Failed to get dynamic type for " + Name);
                        break;
                }
            }
            
        }
        //builder.OpenElement(0, "input");
        //builder.AddAttribute(1, "type", "date");

        object? Value = default;

        object? CurrentValue = prop.GetValue(model);
        if (CurrentValue != default)
        {
            Value = CurrentValue;
        }

        DefaultValueAttribute? Default = prop.GetCustomAttribute<DefaultValueAttribute>();
        if (Default != null)
        {
            Value = Default.Value;
            prop.SetValue(model, Default.Value);
        }

        builder.AddAttribute(2, "value", Value);

        if (!((DynamicFormCodeBase<Model>)form).LoadingSubmit || !item.IsHidden)
        {
            switch (item.ValueType)
            {
                case DynamicValueType.Int:
                    {
                        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
                        {
                            if (value.Value is int)
                            {
                                prop.SetValue(model, prop.GetValue(model));
                                item.IsInvalidValue = false;
                            }
                            else if (value.Value is string str && int.TryParse(str, out int number))
                            {

                                prop.SetValue(model, number);
                                item.IsInvalidValue = false;
                            }
                            else
                                item.IsInvalidValue = true;
                        }));
                    }
                    break;
                case DynamicValueType.Double:
                    {
                        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
                        {
                            if (value.Value is double)
                            {
                                prop.SetValue(model, prop.GetValue(model));
                                item.IsInvalidValue = false;
                            }
                            else if (value.Value is string str && double.TryParse(str, out double number))
                            {

                                prop.SetValue(model, number);
                                item.IsInvalidValue = false;
                            }
                            else
                                item.IsInvalidValue = true;
                        }));
                    }
                    break;
                case DynamicValueType.Short:
                    {
                        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
                        {
                            if (value.Value is short)
                            {
                                prop.SetValue(model, prop.GetValue(model));
                                item.IsInvalidValue = false;
                            }
                            else if (value.Value is string str && short.TryParse(str, out short number))
                            {

                                prop.SetValue(model, number);
                                item.IsInvalidValue = false;
                            }
                            else
                                item.IsInvalidValue = true;
                        }));
                    }
                    break;
                case DynamicValueType.Long:
                    {
                        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
                        {
                            if (value.Value is long)
                            {
                                prop.SetValue(model, prop.GetValue(model));
                                item.IsInvalidValue = false;
                            }
                            else if (value.Value is string str && long.TryParse(str, out long number))
                            {

                                prop.SetValue(model, number);
                                item.IsInvalidValue = false;
                            }
                            else
                                item.IsInvalidValue = true;
                        }));
                    }
                    break;
                case DynamicValueType.Ulong:
                    {
                        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
                        {
                            if (value.Value is ulong)
                            {
                                prop.SetValue(model, prop.GetValue(model));
                                item.IsInvalidValue = false;
                            }
                            else if (value.Value is string str && ulong.TryParse(str, out ulong number))
                            {
                                prop.SetValue(model, number);
                                item.IsInvalidValue = false;
                            }
                            else
                                item.IsInvalidValue = true;
                        }));
                    }
                    break;
                default:
                    {
                        builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
                        {
                            if (value.Value is string str && !string.IsNullOrEmpty(str))
                                prop.SetValue(model, str.Trim());
                            else
                                prop.SetValue(model, value.Value);
                        }));
                    }
                    break;
            }


            //builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
            //{
            //    Console.WriteLine("Set Date: " + value.Value);
            //    if (DateTime.TryParseExact(value.Value as string, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res))
            //        prop.SetValue(model, res);
            //}));

        }

        builder.AddAttribute(5, "name", Name);

        if (item.ValueType == DynamicValueType.Bool)
            builder.AddAttribute(6, "style", "display: block;");
        else if (!item.FieldType.HasFlag(DynamicFieldType.Color))
            builder.AddAttribute(7, "style", "width: calc(100% - 10px)");

        builder.AddAttribute(8, "aria-label", $"{item.Label} Input");

        builder.AddAttribute(9, "id", "dynamicField" + Name);

        if (item.FieldType.HasFlag(DynamicFieldType.Number))
        {
            if (item.Range != null)
            {
                if (item.ValueType == DynamicValueType.Int)
                {
                    builder.AddAttribute(10, "min", (int)item.Range.Minimum);
                    builder.AddAttribute(11, "max", (int)item.Range.Maximum);

                    if (item.RangeStep != null)
                        builder.AddAttribute(12, "step", (int)item.RangeStep.Value);
                }
                else if (item.ValueType == DynamicValueType.Double)
                {
                    builder.AddAttribute(10, "min", (double)item.Range.Minimum);
                    builder.AddAttribute(11, "max", (double)item.Range.Maximum);

                    if (item.RangeStep != null)
                        builder.AddAttribute(12, "step", (double)item.RangeStep.Value);
                }

            }
        }
        else
        {
            MaxLengthAttribute? MaxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (MaxLength != null)
                builder.AddAttribute(10, "maxlength", MaxLength.Length);
            else
            {
                StringLengthAttribute? StringLength = prop.GetCustomAttribute<StringLengthAttribute>();
                if (StringLength != null)
                    builder.AddAttribute(10, "maxlength", StringLength.MaximumLength);
                else
                {
                    LengthAttribute? Length = prop.GetCustomAttribute<LengthAttribute>();
                    if (Length != null)
                        builder.AddAttribute(10, "maxlength", Length.MaximumLength);
                }
            }
        }

        if (item.FieldType.HasFlag(DynamicFieldType.Color))
            builder.AddAttribute(11, "class", "form-control form-control-color");
        else if (item.ValueType == DynamicValueType.Bool)
            builder.AddAttribute(11, "class", "form-check-input");
        else if (item.Range != null)
            builder.AddAttribute(11, "class", "form-range");
        else
            builder.AddAttribute(11, "class", "form-control");

        if (prop.GetCustomAttribute<RequiredAttribute>() != null)
            builder.AddAttribute(12, "required");

        AutoCompleteAttribute? AutoComplete = prop.GetCustomAttribute<AutoCompleteAttribute>();
        if (AutoComplete != null)
            builder.AddAttribute(15, "autocomplete", AutoComplete.Type.GetType().GetMember(AutoComplete.Type.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name);
        else
            builder.AddAttribute(15, "autocomplete", "off");

        ReadOnlyAttribute? ReadOnly = prop.GetCustomAttribute<ReadOnlyAttribute>();
        if (((DynamicFormCodeBase<Model>)form).LoadingSubmit || (ReadOnly != null && ReadOnly.IsReadOnly))
            builder.AddAttribute(16, "readonly");

        PlaceholderAttribute? Placeholder = prop.GetCustomAttribute<PlaceholderAttribute>();
        if (Placeholder != null)
            builder.AddAttribute(17, "placeholder", Placeholder.Value);

        HintsAttribute? FormList = prop.GetCustomAttribute<HintsAttribute>();
        if (FormList != null)
            builder.AddAttribute(18, "list", Name);

        //if (item.PropertyType.HasFlag(FormValueType.File))
        //{
        //    FileCameraAttribute? Camera = prop.GetCustomAttribute<FileCameraAttribute>();
        //    if (Camera != null)
        //        builder.AddAttribute(19, "capture", Camera.Type == FormFileCameraType.Front ? "user" : "environment");
        //}

        if (((DynamicFormCodeBase<Model>)form).LoadingSubmit)
            builder.AddAttribute(20, "disabled");

        IEnumerable<CustomAttribute> Attributes = prop.GetCustomAttributes<CustomAttribute>();
        if (Attributes.Any())
            builder.AddMultipleAttributes(21, Attributes.Select(x => new KeyValuePair<string, object>(x.Name, x.Value)));

        builder.CloseElement();

        if (FormList != null)
        {
            builder.OpenElement(0, "datalist");
            builder.AddAttribute(1, "id", Name);
            if (FormList.Options == null)
                Conditions.NullListOptions(Name);

            foreach (var i in FormList.Options)
            {
                builder.OpenElement(0, "option");
                builder.AddAttribute(1, "value", i);
                builder.CloseElement();
            }
            builder.CloseElement();
        }
    }
}
