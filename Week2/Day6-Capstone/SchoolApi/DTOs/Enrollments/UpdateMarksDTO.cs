using System.ComponentModel.DataAnnotations;

namespace SchoolApi.DTOs.Enrollments;

public class UpdateMarksDTO
{
    [Required]
    public int Marks { get; set; }
}