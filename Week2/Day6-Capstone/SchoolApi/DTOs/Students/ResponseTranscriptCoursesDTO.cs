namespace SchoolApi.DTOs.Students;

public class ResponseTranscriptCoursesDTO
{
    public string TeacherName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int? Marks { get; set; }
    public string? Grade { get; set; } = string.Empty;
}