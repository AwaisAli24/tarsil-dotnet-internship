using SchoolApi.DTOs.Reports;

namespace SchoolApi.Services.Interfaces;

public interface IReportsService
{
    Task<List<ResponseToppersDTO>> GetToppersAsync(int id, int limit);
    Task<ResponseStatsDTO?> GetCourseStatsAsync(int id);
}