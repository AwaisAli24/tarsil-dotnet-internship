using SchoolApi.DTOs.Students;

namespace SchoolApi.Services.Interfaces;

public interface IStudentsService
{
    Task<List<ResponseStudentsDTO>> GetAllAsync(int limit, int offset, int? classRoomId, string? search);
    Task<ResponseStudentsDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateStudentsDTO student);
    Task<bool> UpdateAsync(int id, UpdateStudentsDTO student);
    Task<bool> DeleteAsync(int id);
    Task<ResponseTranscriptDTO?> GetTranscriptAsync(int id);
    Task<bool> ImportStudentDataCSVAsync(IFormFile file);
}