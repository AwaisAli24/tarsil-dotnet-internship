using SchoolApi.DTO.Students;

namespace SchoolApi.Services.Interface;

public interface IStudentsService
{
    Task<List<ResponseStudentsDTO>> GetAllAsync();
    Task<ResponseStudentsDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateStudentsDTO student);
    Task<bool> UpdateAsync(int id, UpdateStudentsDTO student);
    Task<bool> DeleteAsync(int id);
}