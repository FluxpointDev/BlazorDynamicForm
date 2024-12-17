using Microsoft.AspNetCore.Components.Rendering;
using System.Reflection;

namespace BlazorDynamicForm;

public interface IDynamicFormTheme
{
    internal void Build<Model>(RenderTreeBuilder builder, IDynamicFormBase<Model> form, object model, DynamicFormItem item, PropertyInfo prop);
}
