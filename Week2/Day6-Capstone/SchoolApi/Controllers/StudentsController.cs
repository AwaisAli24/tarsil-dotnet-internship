using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO.Students;
using SchoolApi.Services.Interface;

namespace SchoolApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudentsController : ControllerBase
{
    private readonly IStudentsService _service;
    public StudentsController(IStudentsService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        var students = await _service.GetAllAsync();
        return Ok(students);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var student = await _service.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateStudentsDTO student)
    {
        int id;
        try
        {
            id = await _service.CreateAsync(student);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        return Ok($"Student created with id:{id}");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateStudentsDTO student)
    {
        bool isUpdated;
        try
        {
            isUpdated = await _service.UpdateAsync(id, student);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok($"Student updated with id:{id}");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool isDeleted = await _service.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return Ok("Deleted Successfully.");
    }
}