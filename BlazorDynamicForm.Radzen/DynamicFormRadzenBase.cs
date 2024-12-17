using Microsoft.AspNetCore.Components;
using Radzen;

namespace BlazorDynamicForm;

public class DynamicFormRadzenBase<Model> : DynamicFormCodeBase<Model>, IDynamicFormBase<Model>
{
    public DynamicFormRadzenBase()
    {
        IsCustomTheme = true;
    }

    [Parameter]
    public Shade ButtonShade { get; set; } = Shade.Default;

    [Parameter]
    public Shade WarningShade { get; set; } = Shade.Default;

    [Parameter]
    public Variant ButtonVariant { get; set; } = Variant.Filled;

    [Parameter]
    public Variant InputVariant { get; set; } = Variant.Outlined;
}
