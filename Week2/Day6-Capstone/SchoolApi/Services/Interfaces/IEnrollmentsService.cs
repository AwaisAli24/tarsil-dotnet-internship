using SchoolApi.DTOs.Enrollments;
namespace SchoolApi.Services.Interfaces;

public interface IEnrollmentsService
{
    Task<int> AddEnrollmentAsync(CreateEnrollmentsDTO enrollment);
    Task<bool> UpdateMarksAsync(int id, int? marks);
}