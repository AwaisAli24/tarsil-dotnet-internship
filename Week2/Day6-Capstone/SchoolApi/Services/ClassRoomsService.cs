using Npgsql;
using SchoolApi.DTOs.ClassRoom;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services.Interfaces;

namespace SchoolApi.Services;

public class ClassRoomsService : IClassRoomsService
{
    private readonly IClassRoomsRepository _repository;
    public ClassRoomsService(IClassRoomsRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseClassRoomsDTO>> GetAllAsync()
    {
        var classrooms = await _repository.GetAllAsync();
        return classrooms;
    }
    public async Task<ResponseClassRoomsDTO?> GetByIdAsync(int id)
    {
        var classroom = await _repository.GetByIdAsync(id);
        return classroom;
    }
    public async Task<int> CreateAsync(CreateClassRoomsDTO classroom)
    {
        if (classroom.AcademicYear < 0)
        {
            throw new ArgumentException("Invalid Academic Year");
        }
        int id;
        try
        {
            id = await _repository.CreateAsync(classroom);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new InvalidOperationException("ClassRoom with the same name already exists.");
        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            throw new InvalidOperationException("Invalid CourseId. Course does not exist.");
        }
        catch
        {
            throw;
        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateClassRoomsDTO classroom)
    {
        bool isUpdated;
        try
        {

            isUpdated = await _repository.UpdateAsync(id, classroom);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new InvalidOperationException("ClassRoom with the same name already exists.");
        }
        catch
        {
            throw;
        }
        return isUpdated;
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
            throw new InvalidOperationException("ClassRoom cannot be deleted. Relation exists.");
        }
        return isDeleted;
    }
}