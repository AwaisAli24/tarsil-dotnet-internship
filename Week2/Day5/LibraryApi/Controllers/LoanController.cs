using LibraryApi.DTOs;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LoansController : ControllerBase
{
    private readonly ILoanService _service;
    public LoansController(ILoanService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<ActionResult> BorrowAsync(CreateLoanDTO loan)
    {
        int id;
        try
        {
            id = await _service.BorrowAsync(loan);
        }
        catch
        {
            Console.WriteLine("error occured");
            return BadRequest();
        }
        return Ok($"Loan created with id {id}");
    }
}