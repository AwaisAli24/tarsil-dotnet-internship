using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.Models;
using System.Threading.Tasks;

namespace TodoApi.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> Get()
    {
        var todos = await _todoService.GetAsync();
        return Ok(todos);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Todo>> GetByID(int id)
    {
        var todo = await _todoService.GetByIDAsync(id);
        if (todo == null)
        {
            return NotFound();
        }
        return todo;
    }
    [HttpPost]
    public async Task<ActionResult<Todo>> Post(Todo todo)
    {
        if (string.IsNullOrWhiteSpace(todo.Title))
        {
            return BadRequest();
        }
        Todo createdTodo = await _todoService.AddAsync(todo);
        return CreatedAtAction(
            actionName: nameof(GetByID),
            routeValues: new { id = todo.Id },
            value: createdTodo
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Todo>> Put(int id, Todo todo)
    {
        if (string.IsNullOrWhiteSpace(todo.Title))
        {
            return BadRequest();
        }
        Todo? toUpdate = await _todoService.UpdateByIDAsync(id, todo);
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        Todo? toDelete = await _todoService.DeleteByIDAsync(id);
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult> Patch(int id)
    {
        Todo? toComplete = await _todoService.CompleteAsync(id);
        if (toComplete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpGet("completed")]
    public async Task<ActionResult<List<Todo>>> GetCompleted()
    {
        List<Todo> completed = await _todoService.GetCompletedAsync();
        return Ok(completed);
    }
}
