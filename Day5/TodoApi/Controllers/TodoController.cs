using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.Models;
using System.Threading.Tasks;

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
    public async Task<ActionResult<IEnumerable<Todo>>> Get()
    {
        var todos = await _todoService.GetTodos();
        return Ok(todos);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Todo>> GetByID(int id)
    {
        var todo = await _todoService.DeleteTodoByID(id);
        if (todo == null)
        {
            return NotFound();
        }
        return todo;
    }
    [HttpPost]
    public async Task<ActionResult<Todo>> Post(Todo todo)
    {
        if (todo.Title == "")
        {
            return BadRequest();
        }
        Todo createdTodo = await _todoService.PostTodo(todo);
        return CreatedAtAction(
            actionName: "GetById",
            routeValues: new { id = todo.Id },
            value: createdTodo
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Todo>> Put(int id, Todo todo)
    {
        Todo? toUpdate = await _todoService.UpdateTodoByID(id, todo);
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        Todo? toDelete = await _todoService.DeleteTodoByID(id);
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult> Patch(int id)
    {
        Todo? toComplete = await _todoService.Complete(id);
        if (toComplete == null)
        {
            return NotFound();
        }
        return Ok();
    }
    [HttpGet("completed")]
    public async Task<ActionResult<List<Todo>>> GetCompleted()
    {
        List<Todo> completed = await _todoService.GetCompleted();
        return Ok(completed);
    }
}
