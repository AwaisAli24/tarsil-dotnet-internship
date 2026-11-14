using SchoolApi.DTOs.Reports;
using SchoolApi.Repositories.Interfaces;
using SchoolApi.Services.Interfaces;

namespace SchoolApi.Services;

public class ReportsService : IReportsService
{
    private readonly IReportsRepository _repository;
    public ReportsService(IReportsRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseToppersDTO>> GetToppersAsync(int id, int limit)
    {
        var toppers = await _repository.GetToppersAsync(id, limit);
        return toppers;
    }
    public async Task<ResponseStatsDTO?> GetCourseStatsAsync(int id)
    {
        ResponseStatsDTO? stats;
        try
        {

            stats = await _repository.GetCourseStats(id);
        }
        catch
        {
            throw;
        }
        return stats;
    }
}