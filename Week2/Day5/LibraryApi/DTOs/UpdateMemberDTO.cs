using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class UpdateMemberDTO
{
    [Required]
    [MaxLength(50)]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}