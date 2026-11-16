using System.ComponentModel.DataAnnotations;

namespace SchoolApi.DTOs.Students;

public class UpdateStudentsDTO
{
    [Required]
    public string RollNumber { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public int ClassRoomId { get; set; }
    [Required]
    public DateOnly DateOfBirth { get; set; }
}