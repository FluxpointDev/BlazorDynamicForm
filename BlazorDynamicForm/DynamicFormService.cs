namespace BlazorDynamicForm;

public class DynamicFormService
{
    public DynamicFormService(DynamicFormLayout? layout = null)
    {
        DefaultLayout = layout ?? new DynamicFormLayout();
    }

    public DynamicFormLayout DefaultLayout { get; set; }
}
