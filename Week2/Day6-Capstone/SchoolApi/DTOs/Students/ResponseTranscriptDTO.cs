namespace SchoolApi.DTOs.Students;

public class ResponseTranscriptDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<ResponseTranscriptCoursesDTO> courses { get; set; } = new List<ResponseTranscriptCoursesDTO>();
    public double AvgMarks { get; set; }
}