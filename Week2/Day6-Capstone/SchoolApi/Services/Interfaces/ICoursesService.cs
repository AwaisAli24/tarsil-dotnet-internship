using SchoolApi.DTOs.Courses;

namespace SchoolApi.Services.Interfaces;

public interface ICoursesService
{
    Task<List<ResponseCoursesDTO>> GetAllAsync();
    Task<ResponseCoursesDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateCoursesDTO course);
    Task<bool> UpdateAsync(int id, UpdateCoursesDTO course);
    Task<bool> DeleteAsync(int id);
    Task<ResponseRosterDTO?> GetRosterAsync(int id);
}