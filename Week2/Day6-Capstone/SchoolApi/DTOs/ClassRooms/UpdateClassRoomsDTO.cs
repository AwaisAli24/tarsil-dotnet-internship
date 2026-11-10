namespace SchoolApi.DTO.ClassRoom;

public class UpdateClassRoomsDTO
{
    public string Name { get; set; } = string.Empty;
    public int GradeLevel { get; set; }
    public string Section { get; set; } = string.Empty;
    public int AcademicYear { get; set; }
}