
using InventoryApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ResponseCategoryDTO>>> Get()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResponseCategoryDTO?>> GetById(int id)
    {
        var product = await _categoryService.GetCategoryAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<ResponseCategoryDTO>> Add(CreateCategoryDTO category)
    {
        var toAdd = await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = toAdd.Id },
            value: toAdd
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ResponseCategoryDTO?>> Update(int id, UpdateCategoryDTO product)
    {
        var toUpdate = await _categoryService.UpdateCategoryAsync(id, product);
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ResponseCategoryDTO?>> Delete(int id)
    {
        ResponseCategoryDTO? toDelete;
        try
        {
            toDelete = await _categoryService.DeleteCategoryAsync(id);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}