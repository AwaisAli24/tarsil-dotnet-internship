namespace SchoolApi.DTOs.ClassRoom;

public class CreateClassRoomsDTO
{
    public string Name { get; set; } = string.Empty;
    public int GradeLevel { get; set; }
    public string Section { get; set; } = string.Empty;
    public int AcademicYear { get; set; }
}