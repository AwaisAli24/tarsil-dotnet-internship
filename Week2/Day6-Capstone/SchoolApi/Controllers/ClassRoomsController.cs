using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO.ClassRoom;
using SchoolApi.Services.Interface;

namespace SchoolApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ClassRoomsController : ControllerBase
{
    private readonly IClassRoomsService _service;
    public ClassRoomsController(IClassRoomsService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        var classrooms = await _service.GetAllAsync();
        return Ok(classrooms);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResponseClassRoomsDTO?>> GetByIdAsync(int id)
    {
        var classroom = await _service.GetByIdAsync(id);
        if (classroom == null)
        {
            return NotFound();
        }
        return Ok(classroom);
    }
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateClassRoomsDTO course)
    {
        int id;
        try
        {

            id = await _service.CreateAsync(course);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        return Ok($"Course created with Id:{id}");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateClassRoomsDTO classroom)
    {
        bool isUpdated = await _service.UpdateAsync(id, classroom);
        if (!isUpdated)
        {
            return NotFound($"Classroom with id {id} not found.");
        }
        return Ok($"Classroom with id {id} updated.");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool isDeleted = await _service.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound($"Classroom with id {id} not found.");
        }
        return Ok($"Classroom with id {id} deleted successfully.");
    }
}
