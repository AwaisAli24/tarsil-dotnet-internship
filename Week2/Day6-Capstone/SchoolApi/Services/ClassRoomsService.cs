using Npgsql;
using SchoolApi.DTO.ClassRoom;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services.Interface;

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
        catch
        {
            throw;
        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateClassRoomsDTO classroom)
    {
        bool isUpdated = await _repository.UpdateAsync(id, classroom);
        return isUpdated;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);
        return isDeleted;
    }
}