namespace SchoolApi.DTO.Teachers;

public class CreateTeachersDTO
{
    public int EmployeeCode { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public decimal Salary { get; set; }
}