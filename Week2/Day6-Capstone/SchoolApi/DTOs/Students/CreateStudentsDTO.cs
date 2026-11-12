using System.ComponentModel.DataAnnotations;

namespace SchoolApi.DTO.Students;

public class CreateStudentsDTO
{
    [Required]
    public string RollNumber { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public int ClassRoomId { get; set; }
    public DateOnly EnrolledOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);

}