using Npgsql;
using SchoolApi.DTO.ClassRoom;
using SchoolApi.DTO.Courses;

namespace SchoolApi.Repository.Interfaces;

public interface ICoursesRepository
{
    Task<List<ResponseCoursesDTO>> GetAllAsync();
    Task<ResponseCoursesDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateCoursesDTO course);
    Task<bool> UpdateAsync(int id, UpdateCoursesDTO course);
    Task<bool> DeleteAsync(int id);
}