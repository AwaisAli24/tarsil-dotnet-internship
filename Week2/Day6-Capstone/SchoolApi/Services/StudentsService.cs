using SchoolApi.Services.Interfaces;
using SchoolApi.DTOs.Students;
using Npgsql;
using CsvHelper;
public class StudentsService : IStudentsService
{
    private readonly IStudentsRepository _repository;
    public StudentsService(IStudentsRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseStudentsDTO>> GetAllAsync(int limit, int offset, int? classRoomId, string? search)
    {
        var students = await _repository.GetAllAsync(limit, offset, classRoomId, search);
        return students;
    }
    public async Task<ResponseStudentsDTO?> GetByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student;
    }
    public async Task<int> CreateAsync(CreateStudentsDTO student)
    {
        if (student.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("Date of Birth cannot be in the future.");
        }

        int age = DateOnly.FromDateTime(DateTime.Now).Year - student.DateOfBirth.Year;
        if (student.DateOfBirth > DateOnly.FromDateTime(DateTime.Now).AddYears(-age))
        {
            age--;
        }
        if (age < 4 || age > 25)
        {
            throw new ArgumentException("Age must be between 4 and 25 years.");
        }
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
        if (student.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new ArgumentException("Date of Birth cannot be in the future.");
        }
        int age = DateOnly.FromDateTime(DateTime.Now).Year - student.DateOfBirth.Year;
        if (student.DateOfBirth > DateOnly.FromDateTime(DateTime.Now).AddYears(-age))
        {
            age--;
        }
        if (age < 4 || age > 25)
        {
            throw new ArgumentException("Age must be between 4 and 25 years.");
        }
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
    public async Task<ResponseTranscriptDTO?> GetTranscriptAsync(int id)
    {
        ResponseTranscriptDTO? transcript = new ResponseTranscriptDTO();
        try
        {
            transcript = await _repository.GetTranscriptAsync(id);
        }
        catch
        {
            throw;
        }
        return transcript;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<bool> ImportStudentDataCSVAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = CsvReader.Create(reader);
        var students = csv.GetRecords<ImportStudentsDTO>().ToList();
        bool success = await _repository.ImportStudentDataCSVAsync(students);
        return success;
    }
}