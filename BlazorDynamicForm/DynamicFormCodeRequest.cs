using Microsoft.AspNetCore.Components;

namespace BlazorDynamicForm;

public class DynamicFormCodeRequest<Model> : DynamicFormCodeBase<Model>
{
    [Parameter]
    public string Action { get; set; }

    [Parameter]
    public string EncodeType { get; set; }

    [Parameter]
    public string Method { get; set; }

    [Parameter]
    public string Name { get; set; }

    [Parameter]
    public string Rel { get; set; }

    [Parameter]
    public string Target { get; set; }

    [Parameter]
    public string AcceptedCharset { get; set; }
}
