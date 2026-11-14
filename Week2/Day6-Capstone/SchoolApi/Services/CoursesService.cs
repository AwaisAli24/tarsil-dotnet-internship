using SchoolApi.DTOs.Courses;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services.Interfaces;

namespace SchoolApi.Services;

public class CoursesService : ICoursesService
{
    private readonly ICoursesRepository _repository;
    public CoursesService(ICoursesRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseCoursesDTO>> GetAllAsync()
    {
        var courses = await _repository.GetAllAsync();
        return courses;
    }
    public async Task<ResponseCoursesDTO?> GetByIdAsync(int id)
    {
        var course = await _repository.GetByIdAsync(id);
        return course;
    }
    public async Task<int> CreateAsync(CreateCoursesDTO course)
    {
        int id;
        try
        {
            id = await _repository.CreateAsync(course);
        }
        catch
        {
            throw;
        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateCoursesDTO course)
    {
        bool isUpdate = await _repository.UpdateAsync(id, course);
        return isUpdate;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);
        return isDeleted;
    }
    public async Task<ResponseRosterDTO?> GetRosterAsync(int id)
    {
        ResponseRosterDTO? roster = new ResponseRosterDTO();
        try
        {
            roster = await _repository.GetRosterAsync(id);
        }
        catch
        {
            throw;
        }
        return roster;
    }
}