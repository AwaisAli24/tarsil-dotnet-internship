namespace SchoolApi.DTO.ClassRoom;

public class ResponseClassRoomsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeLevel { get; set; }
    public string Section { get; set; } = string.Empty;
    public int AcademicYear { get; set; }
}