using Npgsql;
using SchoolApi.DTOs.Enrollments;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services.Interfaces;

namespace SchoolApi.Services;

public class EnrollmentsService : IEnrollmentsService
{
    private readonly IEnrollmentRepository _repository;
    public EnrollmentsService(IEnrollmentRepository repository)
    {
        _repository = repository;
    }
    public async Task<int> AddEnrollmentAsync(CreateEnrollmentsDTO enrollment)
    {
        int id = 0;
        try
        {
            id = await _repository.AddEnrollmentAsync(enrollment);
        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            if (e.MessageText.Contains("student"))
            {
                throw new InvalidOperationException("Student id does not exist");
            }
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.SqlState);
        }
        catch
        {
            throw;
        }
        return id;
    }
    public async Task<bool> UpdateMarksAsync(int id, int marks)
    {
        if (marks > 100 || marks < 0)
        {
            throw new ArgumentException("Marks should be between 0-100");
        }
        bool isUpdated;
        try
        {

            isUpdated = await _repository.UpdateMarksAsync(id, marks);
        }
        catch
        {
            throw;
        }
        return isUpdated;
    }
}