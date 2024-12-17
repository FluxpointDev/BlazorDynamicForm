using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;
using Radzen.Blazor;
using Microsoft.JSInterop;

namespace BlazorDynamicForm;

public sealed class DynamicRadzenTheme : IDynamicFormTheme
{
    public static DynamicRadzenTheme Instance = new DynamicRadzenTheme();

    public void Build<Model>(RenderTreeBuilder builder, IDynamicFormBase<Model> form, object model, DynamicFormItem item, PropertyInfo prop)
    {
        string Name = prop.Name;

        HintsAttribute? FormList = prop.GetCustomAttribute<HintsAttribute>();

        switch (item.ValueType)
        {
            case DynamicValueType.String:
                {
                    if (item.FieldType.HasFlag(DynamicFieldType.TextArea))
                        builder.OpenComponent<RadzenTextArea>(0);
                    else if (item.FieldType.HasFlag(DynamicFieldType.Password))
                        builder.OpenComponent<RadzenPassword>(0);
                    else if (item.FieldType.HasFlag(DynamicFieldType.Color))
                    {
                        builder.OpenComponent<RadzenColorPicker>(0);
                        builder.AddComponentParameter(1, "ShowRGBA", false);
                    }
                    else if (FormList != null)
                        builder.OpenComponent<RadzenAutoComplete>(0);
                    else
                        builder.OpenComponent<RadzenTextBox>(0);

                    if (item.FieldType.HasFlag(DynamicFieldType.Url))
                            builder.AddComponentParameter(1, "type", "url");
                    else if (item.FieldType.HasFlag(DynamicFieldType.Phone))
                        builder.AddComponentParameter(1, "type", "tel");
                }
                break;
            case DynamicValueType.Bool:
                {
                    builder.OpenComponent<RadzenCheckBox<bool>>(0);
                }
                break;
            case DynamicValueType.Int:
                {
                    if (item.Range != null && item.IsSlider)
                        builder.OpenComponent<RadzenSlider<int>>(0);
                    else
                        builder.OpenComponent<RadzenNumeric<int>>(0);
                }
                break;
            case DynamicValueType.Short:
                {
                    if (item.Range != null && item.IsSlider)
                        builder.OpenComponent<RadzenSlider<short>>(0);
                    else
                        builder.OpenComponent<RadzenNumeric<short>>(0);
                }
                break;
            case DynamicValueType.Long:
                {
                    if (item.Range != null && item.IsSlider)
                        builder.OpenComponent<RadzenSlider<long>>(0);
                    else
                        builder.OpenComponent<RadzenNumeric<long>>(0);
                }
                break;
            case DynamicValueType.Double:
                {
                    if (item.Range != null && item.IsSlider)
                        builder.OpenComponent<RadzenSlider<double>>(0);
                    else
                        builder.OpenComponent<RadzenNumeric<double>>(0);
                }
                break;
            case DynamicValueType.Ulong:
                {
                    if (item.Range != null && item.IsSlider)
                        builder.OpenComponent<RadzenSlider<ulong>>(0);
                    else
                        builder.OpenComponent<RadzenNumeric<ulong>>(0);
                }
                break;
            default:
                throw new Exception("Failed to get dynamic type for " + Name);
        }

        //if (item.FieldType.HasFlag(DynamicFieldType.Select))
        //{
        //    IEnumerable<SelectOptionAttribute> Select = prop.GetCustomAttributes<SelectOptionAttribute>();
        //    builder.OpenElement(0, "select");

        //    foreach (var i in Select)
        //    {
        //        builder.OpenElement(0, "option");
        //        builder.AddContent(1, i.Name);
        //        //builder.AddComponentParameter(3, "value", i.Value);
        //        builder.CloseElement();
        //    }
        //}

        //if (item.PropertyType.HasFlag(FormValueType.Date))
        //{
        //    builder.OpenElement(0, "input");
        //    builder.AddComponentParameter(1, "type", "date");
        //}
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

        builder.AddComponentParameter(2, "value", Value);

        if (!((DynamicFormRadzenBase<Model>)form).LoadingSubmit || !item.IsHidden)
        {
            switch (item.ValueType)
            {
                case DynamicValueType.String:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<string>(form, (value) =>
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (item.FieldType.HasFlag(DynamicFieldType.Color))
                                {
                                    int r;
                                    int g;
                                    int b;
                                    string[] Split = value.Substring(4, value.Length - 5).Split(", ");
                                    r = int.Parse(Split[0]);
                                    g = int.Parse(Split[1]);
                                    b = int.Parse(Split[2]);
                                    
                                    string Hex = string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
                                    prop.SetValue(model, Hex);
                                    ((DynamicFormRadzenBase<Model>)form).JS.InvokeVoidAsync("DynamicFormSetColor", prop.Name, Hex);
                                }   
                                else
                                {
                                    prop.SetValue(model, value);
                                    ((DynamicFormRadzenBase<Model>)form).JS.InvokeVoidAsync("DynamicFormSetColor", prop.Name, "#ffffff");
                                }
                            }
                            else
                                prop.SetValue(model, value);

                        }));
                    }
                    break;
                case DynamicValueType.Bool:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<bool>(form, (value) =>
                        {
                            prop.SetValue(model, value);
                        }));
                    }
                    break;
                case DynamicValueType.Int:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<int>(form, (value) =>
                        {
                            prop.SetValue(model, value);
                        }));
                    }
                    break;
                case DynamicValueType.Short:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<short>(form, (value) =>
                        {
                            prop.SetValue(model, value);
                        }));
                    }
                    break;
                case DynamicValueType.Long:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<long>(form, (value) =>
                        {
                            prop.SetValue(model, value);
                        }));
                    }
                    break;
                case DynamicValueType.Double:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<double>(form, (value) =>
                        {
                            prop.SetValue(model, value);
                        }));
                    }
                    break;
                case DynamicValueType.Ulong:
                    {
                        builder.AddComponentParameter(3, "ValueChanged", EventCallback.Factory.Create<ulong>(form, (value) =>
                        {
                            prop.SetValue(model, value);
                        }));
                    }
                    break;
            }

            



            //if (item.PropertyType.HasFlag(FormValueType.Date))
            //{
            //    builder.AddAttribute(3, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(form, (value) =>
            //    {
            //        Console.WriteLine("Set Date: " + value.Value);
            //        if (DateTime.TryParseExact(value.Value as string, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res))
            //            prop.SetValue(model, res);
            //    }));
            //}


        }

        builder.AddAttribute(5, "name", Name);

        if (item.IsHidden)
            builder.AddComponentParameter(6, "style", "display: none;");
        else if (item.ValueType == DynamicValueType.Bool)
            builder.AddComponentParameter(6, "style", "display: block;");
        else if (!item.FieldType.HasFlag(DynamicFieldType.Color))
            builder.AddComponentParameter(7, "style", "width: calc(100% - 10px)");

        builder.AddComponentParameter(8, "aria-label", $"{item.Label} Input");
        builder.AddAttribute(9, "id", $"dynamicField" + Name);

        if (item.FieldType.HasFlag(DynamicFieldType.Number))
        {
            if (item.Range != null)
            {
                if (item.Range.Minimum != null)
                    builder.AddComponentParameter(10, "Min", Convert.ToDecimal(item.Range.Minimum));

                if (item.Range.Maximum != null)
                    builder.AddComponentParameter(11, "Max", Convert.ToDecimal(item.Range.Maximum));

                if (item.RangeStep != null)
                    builder.AddComponentParameter(12, "Step", item.RangeStep.Value.ToString());
            }

            builder.AddComponentParameter(13, "ShowUpDown", false);
        }
        else
        {
            MaxLengthAttribute? MaxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (MaxLength != null)
            {
                builder.AddComponentParameter(11, "MaxLength", (long?)MaxLength.Length);
            }
            else
            {
                StringLengthAttribute? StringLength = prop.GetCustomAttribute<StringLengthAttribute>();
                if (StringLength != null)
                {
                    builder.AddComponentParameter(11, "MaxLength", (long?)StringLength.MaximumLength);
                }
                else
                {
                    LengthAttribute? Length = prop.GetCustomAttribute<LengthAttribute>();
                    if (Length != null)
                    {
                        builder.AddComponentParameter(11, "MaxLength", (long?)Length.MaximumLength);
                    }
                }
            }
        }

        AutoCompleteAttribute? AutoComplete = prop.GetCustomAttribute<AutoCompleteAttribute>();
        if (AutoComplete != null)
            builder.AddComponentParameter(15, "autocomplete", AutoComplete.Type.GetType().GetMember(AutoComplete.Type.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name);
        else
            builder.AddComponentParameter(15, "autocomplete", "off");

        ReadOnlyAttribute? ReadOnly = prop.GetCustomAttribute<ReadOnlyAttribute>();
        if (ReadOnly != null && ReadOnly.IsReadOnly)
            builder.AddComponentParameter(16, "ReadOnly", true);

        PlaceholderAttribute? Placeholder = prop.GetCustomAttribute<PlaceholderAttribute>();
        if (Placeholder != null)
            builder.AddComponentParameter(17, "Placeholder", Placeholder.Value);

        
        if (FormList != null)
            builder.AddComponentParameter(18, "Data", FormList.Options);

        //if (item.PropertyType.HasFlag(FormValueType.File))
        //{
        //    FileCameraAttribute? Camera = prop.GetCustomAttribute<FileCameraAttribute>();
        //    if (Camera != null)
        //        builder.AddComponentParameter(19, "capture", Camera.Type == FormFileCameraType.Front ? "user" : "environment");
        //}

        if (((DynamicFormRadzenBase<Model>)form).LoadingSubmit)
            builder.AddComponentParameter(20, "Disabled", true);
        else
            builder.AddComponentParameter(20, "Disabled", false);

        builder.AddComponentParameter(21, "Variant", ((DynamicFormRadzenBase<Model>)form).InputVariant);
        

        IEnumerable<CustomAttribute> Attributes = prop.GetCustomAttributes<CustomAttribute>();
        if (Attributes.Any())
            builder.AddMultipleAttributes(22, Attributes.Select(x => new KeyValuePair<string, object>(x.Name, x.Value)));

        builder.CloseComponent();

    }
}

