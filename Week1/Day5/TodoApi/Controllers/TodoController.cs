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
    public async Task<ActionResult<IEnumerable<TodoResponseDto>>> Get([FromQuery] bool? isCompleted, [FromQuery] string? search)
    {
        var todos = await _todoService.GetAsync(isCompleted, search);
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
        return Ok(todo);
    }
    [HttpPost]
    public async Task<ActionResult<TodoResponseDto>> Post(CreateTodoDto dto)
    {
        if (dto.CategoryId == 0)
        {
            return BadRequest("Category Id cannot be Empty.");
        }
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Category cannot be empty");
        }
        TodoResponseDto createdTodo;
        try
        {
            createdTodo = await _todoService.AddAsync(dto);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        return CreatedAtAction(
            actionName: nameof(GetByID),
            routeValues: new { id = createdTodo.Id },
            value: createdTodo
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TodoResponseDto>> Put(int id, UpdateTodoDto dto)
    {
        if (dto.CategoryId == 0)
        {
            return BadRequest("Category Id cannot be Empty.");
        }
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest();
        }
        TodoResponseDto? toUpdate;
        try
        {

            toUpdate = await _todoService.UpdateByIDAsync(id, dto);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var toDelete = await _todoService.DeleteByIDAsync(id);
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult> Patch(int id)
    {
        var toComplete = await _todoService.CompleteAsync(id);
        if (toComplete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpGet("completed")]
    public async Task<ActionResult<List<Todo>>> GetCompleted()
    {
        List<TodoResponseDto> completed = await _todoService.GetCompletedAsync();
        return Ok(completed);
    }
}
