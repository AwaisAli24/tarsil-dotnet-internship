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
        if (loan.days < 1)
        {
            return BadRequest("Days not valid.");
        }
        int id;
        try
        {
            id = await _service.BorrowAsync(loan);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return Ok($"Loan created with id {id}");
    }
    [HttpPost("{id:int}/return")]
    public async Task<ActionResult> ReturnAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id should be greater than 0.");
        }
        try
        {

            await _service.ReturnAsync(id);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        return Ok($"Loan with id {id} has been returned successfully.");
    }
}