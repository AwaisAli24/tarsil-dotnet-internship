using SchoolApi.DTOs.Students;

namespace SchoolApi.DTOs.Courses;

public class ResponseRosterDTO
{
    public string CourseCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; }
    public int Capacity { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public List<ResponseStudentsDTO> EnrolledStudents { get; set; } = new List<ResponseStudentsDTO>();
    public int SeatsLeft { get; set; }
}