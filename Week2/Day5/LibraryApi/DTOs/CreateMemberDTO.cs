using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class CreateMemberDTO
{
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public DateOnly JoinedOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}