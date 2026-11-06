using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs;

namespace LibraryApi.Controllers;

[ApiController]

[Route("/api/[controller]")]

public class ReportsController : ControllerBase
{
    private readonly IReportService _service;
    public ReportsController(IReportService service)
    {
        _service = service;
    }
    [HttpGet("overdue")]
    public async Task<ActionResult<List<ResponseOverdueLoansDTO>>> GetOverdueLoansAsync()
    {
        var loans = await _service.GetOverdueLoansAsync();
        return Ok(loans);
    }
    [HttpGet("top-books")]
    public async Task<ActionResult<List<ResponseTopBorrowedDTO>>> GetMostBorrowedBooks([FromQuery] int? limit)
    {
        var books = await _service.GetMostBorrowedBooks(limit ?? 5);
        return Ok(books);
    }

}