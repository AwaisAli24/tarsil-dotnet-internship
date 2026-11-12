namespace SchoolApi.DTO.Students;

public class ResponseStudentsDTO
{
    public int Id { get; set; }
    public string RollNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int ClassRoomId { get; set; }
    public DateOnly Enrolled_on { get; set; }
}