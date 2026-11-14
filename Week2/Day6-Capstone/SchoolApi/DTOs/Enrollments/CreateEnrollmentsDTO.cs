namespace SchoolApi.DTOs.Enrollments;

public class CreateEnrollmentsDTO
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateOnly EnrolledOn { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}