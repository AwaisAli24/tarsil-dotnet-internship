using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs;
using LibraryApi.Services;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _service;
    public AuthorsController(IAuthorService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult<List<ResponseAuthorDTO>>> GetAllAsync()
    {
        var authors = await _service.GetAllAsync();
        return Ok(authors);
    }
    [HttpGet("{id:int}", Name = "GetAuthorById")]
    public async Task<ActionResult<ResponseAuthorDTOWithBooks?>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0");
        }
        var author = await _service.GetByIdAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }
    [HttpPost]
    public async Task<ActionResult<ResponseAuthorDTO>> CreateAsync(CreateAuthorDTO author)
    {
        ResponseAuthorDTO newAuthor;
        try
        {
            newAuthor = await _service.CreateAsync(author);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtRoute(
            routeName: "GetAuthorById",
            routeValues: new { id = newAuthor.Id },
            value: newAuthor
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateAuthorDTO author)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0");
        }
        ResponseAuthorDTO? updatedAuthor;
        try
        {
            updatedAuthor = await _service.UpdateAsync(id, author);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        if (updatedAuthor == null)
        {
            return NotFound();
        }
        return Ok(updatedAuthor);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0");
        }
        bool toDelete;
        try
        {
            toDelete = await _service.DeleteAsync(id);
        }
        catch (ArgumentException e)
        {
            return Conflict(e.Message);
        }
        if (!toDelete)
        {
            return NotFound();
        }
        return NoContent();
    }
}