using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlazorDynamicForm.Demo;

public class RadzenModel
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name requires minimum of 3 characters")]
    [MaxLength(32, ErrorMessage = "Name has a maximum of 32 characters")]
    [DisplayName("Custom Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [AutoComplete(AutoCompleteType.Email)]
    [Description("Email type with auto trim whitespaces")]
    public string Email { get; set; }

    [StringLength(300, MinimumLength = 1, ErrorMessage = "Description has a maximum of 300 characters")]
    [FieldTextArea]
    [Placeholder("Hi")]
    [Description("Maximum characters 300")]
    public string Description { get; set; }

    [DefaultValue(1)]
    public int Number { get; set; }

    public double DoubleNumber { get; set; } = 10.69;

    public bool Checkbox { get; set; }

    [Color]
    [Description("Value is shown as HEX #FFFFFF")]
    public string Color { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [PasswordPropertyText]
    [Description("This is a masked input")]
    public string Password { get; set; }

    [Range(0, 100)]
    [RangeSlider]
    [RangeStep(10)]
    public int Range { get; set; }

    [Hints(new string[] { "Cat", "Dog", "Snake", "Sheep", "Duck", "Horse", "Lizard", "Bird", "Penguin", "Fish" })]
    [Description("This has a list of hints")]
    public string Animal { get; set; }

    [ImageUrl]
    [HttpsUrl]
    public string Image { get; set; }

    [Hidden]
    public string Hidden { get; set; }

    [ReadOnly(true)]
    public string ReadOnly { get; set; } = "Copy me!";
}
