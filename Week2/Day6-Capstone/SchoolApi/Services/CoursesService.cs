using SchoolApi.DTOs.Courses;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services.Interfaces;
using Npgsql;

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
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new InvalidOperationException("Course with the same code already exists.");
        }
        catch (PostgresException e) when (e.SqlState == "23503" || e.SqlState == "23001")
        {
            throw new InvalidOperationException("Cannot create course because the associated teacher does not exist.");
        }
        catch
        {
            throw;
        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateCoursesDTO course)
    {
        bool isUpdate;
        try
        {

            isUpdate = await _repository.UpdateAsync(id, course);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new InvalidOperationException("Course with the same code already exists.");
        }
        catch (PostgresException e) when (e.SqlState == "23503" || e.SqlState == "23001")
        {
            throw new InvalidOperationException("Cannot create course because the associated teacher does not exist.");
        }
        catch
        {
            throw;
        }
        return isUpdate;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted;
        try
        {

            isDeleted = await _repository.DeleteAsync(id);
        }
        catch (PostgresException e) when (e.SqlState == "23503" || e.SqlState == "23001")
        {
            throw new InvalidOperationException("Cannot delete course because it has associated students.");
        }
        catch
        {
            throw;
        }
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