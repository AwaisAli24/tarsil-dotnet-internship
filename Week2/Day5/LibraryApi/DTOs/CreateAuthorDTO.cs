using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class CreateAuthorDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}