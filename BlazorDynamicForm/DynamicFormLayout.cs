namespace BlazorDynamicForm;

public class DynamicFormLayout
{
    public bool ButtonShowReset { get; set; }
    public string ButtonSubmitText { get; set; } = "Submit";
    public string ButtonResetText { get; set; } = "Reset";

    public DynamicFormLayoutFlex ActionsFlex { get; set; } = DynamicFormLayoutFlex.Start;

    internal string GetFlex()
    {
        switch(ActionsFlex)
        {
            case DynamicFormLayoutFlex.Center:
                return "justify-content: center;";
            case DynamicFormLayoutFlex.End:
                return "justify-content: flex-end;";
        }
        return "justify-content: flex-start;";
    }
}
public enum DynamicFormLayoutFlex
{
    Start, Center, End
}
