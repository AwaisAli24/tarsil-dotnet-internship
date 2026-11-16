using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTOs.Reports;
using SchoolApi.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _service;
    public ReportsController(IReportsService service)
    {
        _service = service;
    }
    [HttpGet("classrooms/{id:int}/toppers")]
    public async Task<ActionResult> GetToppersAsync(int id, [FromQuery] int limit = 3)
    {
        if (limit < 1)
        {
            return BadRequest("Limit cannot be zero or negative.");
        }
        var toppers = await _service.GetToppersAsync(id, limit);
        if (toppers.Count == 0)
        {
            return NotFound();
        }
        return Ok(toppers);
    }
    [HttpGet("courses/{id:int}/stats")]
    public async Task<ActionResult> GetCourseStatsAsync(int id)
    {
        ResponseStatsDTO? stats;
        try
        {
            stats = await _service.GetCourseStatsAsync(id);
        }
        catch
        {
            throw;
        }
        if (stats == null)
        {
            return NotFound();
        }
        return Ok(stats);
    }
}