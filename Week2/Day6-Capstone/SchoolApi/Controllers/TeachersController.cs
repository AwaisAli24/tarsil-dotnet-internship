using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO.Teachers;
using SchoolApi.Services.Interface;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;
    public TeachersController(ITeacherService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        var teachers = await _service.GetAllAsync();
        return Ok(teachers);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var teacher = await _service.GetByIdAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }
        return Ok(teacher);
    }
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateTeachersDTO teacher)
    {
        int id;
        try
        {
            id = await _service.CreateAsync(teacher);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        return Ok($"Teacher created with id {id}");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateTeachersDTO teacher)
    {
        bool isUpdated;
        try
        {
            isUpdated = await _service.UpdateAsync(id, teacher);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok($"Teacher with id {id} updated.");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool isDeleted;
        try
        {

            isDeleted = await _service.DeleteAsync(id);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        if (!isDeleted)
        {
            return NotFound();
        }
        return Ok($"Teacher with id {id} deleted successfully.");
    }
}