using Microsoft.AspNetCore.Mvc;

namespace BlazorDynamicForm.Demo.Controllers;
public class TestController : Controller
{
    [HttpPost("/api/post")]
    public IActionResult Post([FromForm] TestModel data)
    {
        Console.WriteLine("Data has been posted");
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        return Json(data);
    }
}
