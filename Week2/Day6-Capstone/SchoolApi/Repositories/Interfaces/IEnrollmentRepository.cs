using SchoolApi.DTOs.Enrollments;
namespace SchoolApi.Repository.Interfaces;

public interface IEnrollmentRepository
{
    Task<int> AddEnrollmentAsync(CreateEnrollmentsDTO enrollment);
    Task<bool> UpdateMarksAsync(int id, int marks);
}