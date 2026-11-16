using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTOs.Enrollments;
using SchoolApi.Services.Interfaces;

namespace SchoolApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentsService _service;
    public EnrollmentsController(IEnrollmentsService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<ActionResult> AddEnrollmentAsync(CreateEnrollmentsDTO enrollment)
    {
        int id;
        try
        {
            id = await _service.AddEnrollmentAsync(enrollment);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return Ok($"Enrollment created with id {id}");
    }
    [HttpPut("{id:int}/marks")]
    public async Task<ActionResult> UpdateMarksAsync(int id, UpdateMarksDTO body)
    {
        bool isUpdated;
        try
        {
            isUpdated = await _service.UpdateMarksAsync(id, body.Marks);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok($"Marks updated in enrollment id {id}");
    }
}