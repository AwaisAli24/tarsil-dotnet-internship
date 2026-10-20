using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.Models;

namespace TodoApi.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TodoApiController : ControllerBase
{
    public ITodoService _todoService;
    public TodoApiController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Todo>> Get()
    {
        return Ok(_todoService.GetTodos());
    }
    [HttpGet("{id:int}")]
    public ActionResult<Todo> GetByID(int id)
    {
        var todo = _todoService.DeleteTodoByID(id);
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
        Todo createdTodo = _todoService.PostTodo(todo);
        return CreatedAtAction(
            actionName: "GetById",
            routeValues: new { id = todo.Id },
            value: createdTodo
        );
    }
    [HttpPut("{id:int}")]
    public ActionResult<Todo> Put(int id, Todo todo)
    {
        Todo? toUpdate = _todoService.UpdateTodoByID(id, todo);
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        Todo? toDelete = _todoService.DeleteTodoByID(id);
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPatch("{id:int}/complete")]
    public ActionResult Patch(int id)
    {
        Todo? toComplete = _todoService.Complete(id);
        if (toComplete == null)
        {
            return NotFound();
        }
        return Ok();
    }
    [HttpGet("completed")]
    public ActionResult<List<Todo>> GetCompleted()
    {
        List<Todo> completed = _todoService.GetCompleted();
        return Ok(completed);
    }
}
