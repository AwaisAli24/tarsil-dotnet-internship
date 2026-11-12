using SchoolApi.DTO.Teachers;

namespace SchoolApi.Services.Interface;

public interface ITeacherService
{
    Task<List<ResponseTeacherDTO>> GetAllAsync();
    Task<ResponseTeacherDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateTeachersDTO teacher);
    Task<bool> UpdateAsync(int id, UpdateTeachersDTO teacher);
    Task<bool> DeleteAsync(int id);
}