namespace BlazorDynamicForm;

public static class DynamicFormExtensions
{
    public static DynamicFormService AddDynamicForm(this IServiceCollection services, DynamicFormLayout? layout = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        DynamicFormService Service = new DynamicFormService(layout);
        services.AddSingleton(Service);
        return Service;
    }
}
