using SchoolApi.DTOs.Teachers;

namespace SchoolApi.Services.Interfaces;

public interface ITeacherService
{
    Task<List<ResponseTeacherDTO>> GetAllAsync();
    Task<ResponseTeacherDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateTeachersDTO teacher);
    Task<bool> UpdateAsync(int id, UpdateTeachersDTO teacher);
    Task<bool> DeleteAsync(int id);
}