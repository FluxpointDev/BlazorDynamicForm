using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Reflection;

namespace BlazorDynamicForm;

public class DynamicFormCodeBase<Model> : ComponentBase, IDynamicFormBase<Model>
{
    internal DynamicFormService? Service { get; private set; }

    [Parameter]
    public Model ModelData { get; set; }

    public Model CloneData { get; internal set; }

    internal bool IsCustomTheme { get; set; }

    // public EditForm Form { get; internal set; }

    public List<DynamicFormItem> FormItems { get; internal set; }

    // [Parameter]
    // public string Description { get; set; }

    [Parameter]
    public Func<Model, Task<string?>> OnSubmit { get; set; }

    [Parameter]
    public Func<Model, DynamicFormError[], Task> OnError { get; set; }

    [Parameter]
    public DynamicFormLayout Layout { get; set; }

    [Parameter]
    public Dictionary<string, Func<Model, Task<string?>>>? CustomActions { get; set; } = new Dictionary<string, Func<Model, Task<string?>>>();

    public string? ErrorMessage { get; set; }

    internal bool LoadingSubmit = false;

    public RenderFragment RenderFormElement(IDynamicFormTheme theme, object model, DynamicFormItem item, PropertyInfo prop) => builder =>
    {
        theme.Build(builder, this, model, item, prop);
    };

    [Inject]
    internal IServiceProvider Services { get; set; }

    [Inject]
    internal IJSRuntime JS { get; set; }

    [Inject]
    internal NavigationManager Nav { get; set; }

    protected override void OnInitialized()
    {
        Service = Services.GetService<DynamicFormService>();
    }


    protected override void OnParametersSet()
    {
        if (Service == null)
            return;

        shouldRender = true;
        if (Layout == null)
        {
            if (Service.DefaultLayout == null)
                Service.DefaultLayout = new DynamicFormLayout();
            Layout = Service.DefaultLayout;
        }
        ErrorMessage = null;
        if (ModelData != null)
        {
            CloneData = JsonConvert.DeserializeObject<Model>(JsonConvert.SerializeObject(ModelData));
            FormItems = CloneData.GetType().GetProperties().Select(x => new DynamicFormItem(CloneData, x)).ToList();
        }
        shouldRender = false;
    }


    internal bool shouldRender;
    protected override bool ShouldRender() => shouldRender;

    public async Task SubmitData()
    {
        if (LoadingSubmit)
            return;

        shouldRender = true;
        StateHasChanged();
        LoadingSubmit = true;
        bool IsValid = true;
        foreach (var i in FormItems)
        {
            try
            {
                if (!i.Validate(CloneData))
                {
                    IsValid = false;
                }
                else if (i.IsInvalidValue)
                {
                    i.ErrorMessage = "Invalid value";
                    IsValid = false;
                }
                else
                {
                    i.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                i.ErrorMessage = "Failed to validate, " + ex.Message;
            }
        }

        // Debug purposes
        //IsValid = true;

        if (!IsValid)
        {
            OnError?.Invoke(CloneData, FormItems.Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).Select(x => new DynamicFormError(x)).ToArray());
            LoadingSubmit = false;
            StateHasChanged();
            shouldRender = false;
            return;
        }
        if (OnSubmit != null)
        {
            try
            {
                ErrorMessage = await OnSubmit?.Invoke(CloneData);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to submit, " + ex.Message;
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                OnError?.Invoke(CloneData, new DynamicFormError[] { new DynamicFormError() { ErrorMessage = ErrorMessage } });
            }
        }
        LoadingSubmit = false;
        StateHasChanged();
        shouldRender = false;
    }

    public async Task CustomAction(string name)
    {
        if (CustomActions != null && CustomActions.TryGetValue(name, out var function))
        {
            shouldRender = true;
            StateHasChanged();
            LoadingSubmit = true;

            try
            {
                ErrorMessage = await function?.Invoke(CloneData);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to run action {name}, {ex.Message}";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
                OnError?.Invoke(CloneData, new DynamicFormError[] { new DynamicFormError() { CustomAction = name, ErrorMessage = ErrorMessage } });

            LoadingSubmit = false;
            StateHasChanged();
            shouldRender = false;
        }
    }

    public async Task Reset()
    {
        shouldRender = true;
        CloneData = JsonConvert.DeserializeObject<Model>(JsonConvert.SerializeObject(ModelData));
        FormItems = CloneData.GetType().GetProperties().Select(x => new DynamicFormItem(CloneData, x)).ToList();
        if (!IsCustomTheme)
        {
            foreach (var i in ModelData.GetType().GetProperties())
            {
                try
                {
                    await JS.InvokeVoidAsync("DynamicFormSetField", i.Name, i.GetValue(ModelData));
                }
                catch { }
            }
        }
        StateHasChanged();
        shouldRender = false;
        return;
    }
}
internal enum DynamicValueType
{
    String,
    Short,
    Int,
    Long,
    Double,
    Ulong,
    //Date,
    Bool
}
[Flags]
internal enum DynamicFieldType
{
    String = 1 << 0,
    Number = 1 << 1,
    Checkbox = 1 << 2,
    File = 1 << 3,
    Select = 1 << 4,
    Color = 1 << 5,
    Password = 1 << 6,
    TextArea = 1 << 7,
    Url = 1 << 8,
    Phone = 1 << 9,
    //Date = 1 << 7
}
