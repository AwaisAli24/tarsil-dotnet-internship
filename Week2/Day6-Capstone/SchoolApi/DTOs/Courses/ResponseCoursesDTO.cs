namespace SchoolApi.DTOs.Courses;

public class ResponseCoursesDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; }
    public int Capacity { get; set; }
    public int TeacherId { get; set; }
}