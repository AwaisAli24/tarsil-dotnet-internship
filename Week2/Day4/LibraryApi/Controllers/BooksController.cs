using Microsoft.AspNetCore.Mvc;
using LibraryApi.Services.Interfaces;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    public BooksController(IBookService service)
    {
        _service = service;
    }
    // [HttpGet(Name = "GetWeatherForecast")]
}
