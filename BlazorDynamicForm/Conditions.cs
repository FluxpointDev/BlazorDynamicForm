namespace BlazorDynamicForm;

public static class Conditions
{
    public static void NotValidRangeType(string name)
    {
        throw new ArgumentException($"{name} is not a valid range input type.");
    }

    public static void NullListOptions(string name)
    {
        throw new ArgumentException($"{name} has a missing list options.");
    }
}
