using System.ComponentModel.DataAnnotations;

namespace SchoolApi.DTO.Courses;

public class CreateCoursesDTO
{
    [Required]
    public string Code { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    [Range(1, 6)]
    public int CreditHours { get; set; }
    [Required]
    [Range(1, 60)]
    public int Capacity { get; set; }
    [Required]
    public int TeacherId { get; set; }
}