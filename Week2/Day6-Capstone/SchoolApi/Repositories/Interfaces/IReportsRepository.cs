using SchoolApi.DTOs.Reports;
namespace SchoolApi.Repositories.Interfaces;

public interface IReportsRepository
{
    Task<List<ResponseToppersDTO>> GetToppersAsync(int id, int limit);
    Task<ResponseStatsDTO?> GetCourseStats(int id);
}