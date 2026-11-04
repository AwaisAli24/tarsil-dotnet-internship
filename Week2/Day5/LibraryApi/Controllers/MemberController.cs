using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs;

namespace LibraryApi.Controllers;

[ApiController]

[Route("/api/[controller]")]

public class MembersController : ControllerBase
{
    private readonly IMemberService _service;
    public MembersController(IMemberService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult<List<ResponseMemberDTO>>> GetAllAsync()
    {
        var members = await _service.GetAllAsync();
        return Ok(members);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResponseMemberDTO>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }
        var member = await _service.GetByIdAsync(id);
        if (member == null)
        {
            return NotFound();
        }
        return Ok(member);
    }
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateMemberDTO member)
    {
        int id;
        try
        {
            id = await _service.CreateAsync(member);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return Ok($"Member successfully created with id: {id}");
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateMemberDTO member)
    {
        bool affectedRows;
        try
        {
            affectedRows = await _service.UpdateAsync(id, member);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        if (!affectedRows)
        {
            return NotFound();
        }
        return Ok($"Member with id {id} updated successfully");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        bool affectedRows;
        try
        {
            affectedRows = await _service.DeleteAsync(id);
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
        if (!affectedRows)
        {
            return NotFound();
        }
        return Ok($"Member with id {id} deleted successfully.");
    }
}