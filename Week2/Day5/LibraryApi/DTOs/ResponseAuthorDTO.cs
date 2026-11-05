using LibraryApi.Models;

namespace LibraryApi.DTOs;

public class ResponseAuthorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}