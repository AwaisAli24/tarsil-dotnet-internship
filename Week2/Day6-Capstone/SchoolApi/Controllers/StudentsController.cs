using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTOs.Students;
using SchoolApi.Services.Interfaces;
using CsvHelper;
using Microsoft.AspNetCore.Http.HttpResults;

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
    public async Task<ActionResult> GetAllAsync([FromQuery] int? classRoomId, [FromQuery] string? search, [FromQuery] int? pageSize, [FromQuery] int? page)
    {
        if (pageSize.HasValue && pageSize < 1)
        {
            return BadRequest("Page size cannot be zero or negative.");
        }
        if (page.HasValue && page < 1)
        {
            return BadRequest("Page number cannot be zero or negative");
        }
        if (classRoomId.HasValue && classRoomId < 1)
        {
            return BadRequest("Class Room id cannot be zero or negative.");
        }
        int limit = pageSize ?? 10;
        int offset = ((page ?? 1) - 1) * limit;

        var students = await _service.GetAllAsync(limit, offset, classRoomId, search);
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
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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
        return Ok("Deleted Successfully.");
    }
    [HttpGet("{id:int}/transcript")]
    public async Task<ActionResult> GetTranscriptAsync(int id)
    {
        ResponseTranscriptDTO? transcript = await _service.GetTranscriptAsync(id);
        if (transcript == null)
        {
            return NotFound();
        }
        return Ok(transcript);

    }
    [HttpPost("import")]
    public async Task<ActionResult> ImportStudentDataCSV(IFormFile file)
    {
        bool success = await _service.ImportStudentDataCSVAsync(file);
        if (!success)
        {
            throw new InvalidOperationException("Unable to import student data.");
        }
        return Ok();
    }
}