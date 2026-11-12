namespace SchoolApi.DTO.Teachers;

public class ResponseTeacherDTO
{
    public int Id { get; set; }
    public int EmployeeCode { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public decimal Salary { get; set; }
}