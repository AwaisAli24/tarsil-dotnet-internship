namespace SchoolApi.DTOs.Students;

public class ResponseTranscriptDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<CompletedTranscriptCoursesDTO> CompletedCourses { get; set; } = new List<CompletedTranscriptCoursesDTO>();
    public List<InProgressTranscriptCoursesDTO> InProgressCourses { get; set; } = new List<InProgressTranscriptCoursesDTO>();
    public double AvgMarks { get; set; }
}

public class CompletedTranscriptCoursesDTO
{
    public string TeacherName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int Marks { get; set; }
    public string Grade { get; set; } = string.Empty;
}
public class InProgressTranscriptCoursesDTO
{
    public string TeacherName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
}