using SchoolApi.Services.Interface;
using SchoolApi.DTO.Students;
using Npgsql;

public class StudentsService : IStudentsService
{
    private readonly IStudentsRepository _repository;
    public StudentsService(IStudentsRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseStudentsDTO>> GetAllAsync()
    {
        var students = await _repository.GetAllAsync();
        return students;
    }
    public async Task<ResponseStudentsDTO?> GetByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student;
    }
    public async Task<int> CreateAsync(CreateStudentsDTO student)
    {
        int id;
        try
        {
            id = await _repository.CreateAsync(student);
        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            throw new InvalidOperationException("The class id provided Does not exist");
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            if (e.MessageText.Contains("roll_number"))
            {
                throw new InvalidOperationException("Roll number should be Unique.");
            }
            else //(e.MessageText.Contains("email"))
            {
                throw new InvalidOperationException("Email Should be unique.");
            }

        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateStudentsDTO student)
    {
        bool isUpdated;
        try
        {
            isUpdated = await _repository.UpdateAsync(id, student);
        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            throw new InvalidOperationException("The class id provided Does not exist");
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            if (e.MessageText.Contains("roll_number"))
            {
                throw new InvalidOperationException("Roll number should be Unique.");
            }
            else //(e.MessageText.Contains("email"))
            {
                throw new InvalidOperationException("Email Should be unique.");
            }

        }
        return isUpdated;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);
        return isDeleted;
    }
}