using Npgsql;
using SchoolApi.DTO.Teachers;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services.Interface;

namespace SchoolApi.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeachersRepository _repository;
    public TeacherService(ITeachersRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseTeacherDTO>> GetAllAsync()
    {
        var teachers = await _repository.GetAllAsync();
        return teachers;
    }
    public async Task<ResponseTeacherDTO?> GetByIdAsync(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);
        return teacher;
    }
    public async Task<int> CreateAsync(CreateTeachersDTO teacher)
    {
        int id;
        try
        {
            id = await _repository.CreateAsync(teacher);
            return id;
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            if (e.MessageText.Contains("employee_code"))
            {
                throw new InvalidOperationException("Employee Code should be unique.");
            }
            else if (e.MessageText.Contains("email"))
            {
                throw new InvalidOperationException("Email Already Exist! Enter a different email address.");
            }
            else if (e.MessageText.Contains("phone"))
            {
                throw new InvalidOperationException("Phone Number Already Exist! Enter a different phone number.");
            }
            else throw;
        }
    }
    public async Task<bool> UpdateAsync(int id, UpdateTeachersDTO teacher)
    {
        bool isUpdated;
        try
        {
            isUpdated = await _repository.UpdateAsync(id, teacher);
            return isUpdated;
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            if (e.MessageText.Contains("employee_code"))
            {
                throw new InvalidOperationException("Employee Code should be unique.");
            }
            else if (e.MessageText.Contains("email"))
            {
                throw new InvalidOperationException("Email Already Exist! Enter a different email address.");
            }
            else if (e.MessageText.Contains("phone"))
            {
                throw new InvalidOperationException("Phone Number Already Exist! Enter a different phone number.");
            }
            else throw;
        }
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
            throw new InvalidOperationException("Cannot delete teacher because it is referenced by other records.");
        }
        return isDeleted;
    }
}