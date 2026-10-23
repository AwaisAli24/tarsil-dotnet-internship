using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    public async Task<ActionResult<List<Category>>> Get()
    {
        var categories = await _categoryService.GetAsync();
        return Ok(categories);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }
    [HttpPost]
    public async Task<ActionResult<Category>> Post(Category category)
    {
        var toPost = await _categoryService.AddAsync(category);
        return CreatedAtAction(actionName: nameof(GetById), routeValues: new { id = toPost.Id }, value: toPost);
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Category>> Put(int id, Category category)
    {
        var toUpdate = _categoryService.UpdateAsync(id, category);
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Category>> Delete(int id)
    {
        Category? toDelete;
        try
        {
            toDelete = await _categoryService.DeleteAsync(id);
        }
        catch
        {
            return Conflict();
        }
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpGet("{id:int}/todos")]
    public async Task<ActionResult<List<Category>>> GetTodosByCategory(int id)
    {
        var todos = await _categoryService.GetTodosAsync(id);
        if (todos == null)
        {
            return NotFound();
        }
        return Ok(todos);
    }
}