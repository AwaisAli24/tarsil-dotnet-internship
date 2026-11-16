using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTOs.Courses;
using SchoolApi.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICoursesService _service;
    public CoursesController(ICoursesService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        var courses = await _service.GetAllAsync();
        return Ok(courses);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var course = await _service.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return Ok(course);
    }
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateCoursesDTO course)
    {
        int id;
        try
        {

            id = await _service.CreateAsync(course);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        return Ok($"Course created with Id {id}");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateCoursesDTO course)
    {
        bool isUpdated = await _service.UpdateAsync(id, course);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok($"Course with id {id} updated.");
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
        return Ok($"Course with id {id} Deleted successfully.");
    }
    [HttpGet("{id:int}/roster")]
    public async Task<ActionResult> GetRosterAsync(int id)
    {
        ResponseRosterDTO? roster = new ResponseRosterDTO();
        roster = await _service.GetRosterAsync(id);
        if (roster == null)
        {
            return NotFound();
        }
        return Ok(roster);
    }
}