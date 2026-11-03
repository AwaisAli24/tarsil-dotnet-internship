using Microsoft.AspNetCore.Mvc;
using LibraryApi.Services.Interfaces;
using LibraryApi.DTOs;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    public BooksController(IBookService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult<List<ResponseBookDTO>>> GetAllAsync([FromQuery] string? title, [FromQuery] int? authorId, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var books = await _service.GetAllAsync(page ?? 1, pageSize ?? 10, title, authorId, minPrice, maxPrice);
        return Ok(books);
    }
    [HttpGet("{id:int}", Name = "GetById")]
    public async Task<ActionResult<ResponseBookDTO?>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0");
        }
        var book = await _service.GetByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
    [HttpPost]
    public async Task<ActionResult<ResponseBookDTO>> CreateAsync(CreateBookDTO book)
    {
        ResponseBookDTO newBook;
        try
        {
            newBook = await _service.CreateAsync(book);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(
            actionName: "GetById",
            routeValues: new { id = newBook.Id },
            value: newBook
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, UpdateBookDTO book)
    {
        if (id > 0)
        {
            return BadRequest("Id must be greater than 0");
        }
        var toUpdate = await _service.UpdateAsync(id, book);
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var toDelete = await _service.DeleteAsync(id);
        if (!toDelete)
        {
            return NotFound();
        }
        return NoContent();
    }
}
