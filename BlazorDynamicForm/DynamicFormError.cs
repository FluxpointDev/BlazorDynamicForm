namespace BlazorDynamicForm;

public class DynamicFormError
{
    public DynamicFormError()
    {

    }

    public DynamicFormError(DynamicFormItem item)
    {
        ErrorMessage = item.ErrorMessage;
        Item = item;
    }

    public string? CustomAction { get; internal set; }

    public string ErrorMessage { get; internal set; }

    public DynamicFormItem? Item { get; private set; }
}
