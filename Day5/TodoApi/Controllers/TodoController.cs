using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.Models;
using System.Threading.Tasks;
using TodoApi.Dto;

namespace TodoApi.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;
    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoResponseDto>>> Get()
    {
        var todos = await _todoService.GetAsync();
        return Ok(todos);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoResponseDto>> GetByID(int id)
    {
        var todo = await _todoService.GetByIDAsync(id);
        if (todo == null)
        {
            return NotFound();
        }
        Console.WriteLine($"{todo.Title} From Controller");
        return Ok(todo);
    }
    [HttpPost]
    public async Task<ActionResult<Todo>> Post(CreateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest();
        }
        Todo createdTodo = await _todoService.AddAsync(dto);
        return CreatedAtAction(
            actionName: nameof(GetByID),
            routeValues: new { id = createdTodo.Id },
            value: createdTodo
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Todo>> Put(int id, UpdateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest();
        }
        Todo? toUpdate = await _todoService.UpdateByIDAsync(id, dto);
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
