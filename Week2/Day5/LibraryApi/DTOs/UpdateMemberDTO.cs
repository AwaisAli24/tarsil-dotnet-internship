using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class UpdateMemberDTO
{
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
}