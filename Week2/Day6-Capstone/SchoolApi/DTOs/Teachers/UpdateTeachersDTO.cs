namespace SchoolApi.DTO.Teachers;

public class UpdateTeachersDTO
{
    public int EmployeeCode { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}