using System.ComponentModel.DataAnnotations;
using CsvHelper;

namespace SchoolApi.DTOs.Students;

[CsvRecord]
public partial class ImportStudentsDTO
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