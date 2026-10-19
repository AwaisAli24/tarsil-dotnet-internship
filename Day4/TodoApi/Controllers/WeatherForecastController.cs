using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TodoApiController : ControllerBase
{
    public static List<Todo> todos = new() {
        new Todo(1, "Learn .NET Core",
        "Learn .NET Core by the end of this month",
        false, DateTime.Now),

    new Todo(2, "Get Better at .NET Core",
        "Master .NET Core in less than 3 months",
        false, DateTime.Now)
    };


    public TodoApiController()
    {
        // Todo Todo1 = new Todo(1, "Learn .NET Core", "Learn .NET Core by the end of this month", false, DateTime.Now);
        // Todo Todo2 = new Todo(2, "Get Better at .NET Core", "Master .NET Core in less than 3 months", false, DateTime.Now);
        // todos.Add(Todo1);
        // todos.Add(Todo2);
    }

    [HttpGet(Name = "GetTodoApi")]
    public ActionResult<IEnumerable<Todo>> Get()
    {
        Console.WriteLine(todos.Count);
        return Ok(todos);
    }
    [HttpGet("{id:int}", Name = "GetById")]
    public ActionResult<Todo> GetByID(int id)
    {
        var todo = todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        return todo;
    }
    [HttpPost]
    public ActionResult<Todo> Post(Todo todo)
    {
        if (todo.Title == "")
        {
            return BadRequest();
        }

        todo.CreatedAt = DateTime.Now;
        todo.Id = todos.Max(t => t.Id) + 1;
        todos.Add(todo);
        return CreatedAtAction(
            actionName: "GetById",
            routeValues: new { id = todo.Id },
            value: todo
        );
    }
    [HttpPut("{id:int}")]
    public ActionResult<Todo> Put(int id, Todo todo)
    {
        var toModify = todos.FirstOrDefault(t => t.Id == id);
        if (toModify == null)
        {
            return NotFound();
        }
        toModify.Title = todo.Title;
        toModify.Description = todo.Description;
        toModify.IsCompleted = todo.IsCompleted;
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public ActionResult<Todo> Delete(int id)
    {
        var toDelete = todos.FirstOrDefault(t => t.Id == id);
        if (toDelete == null)
        {
            return NotFound();
        }
        todos.Remove(toDelete);
        return NoContent();
    }
}
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public Todo(int id, string title, string description, bool isCompleted, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
    }
}