using Npgsql;
using SchoolApi.DTOs.Teachers;
using SchoolApi.Repository.Interfaces;

public class TeachersRepository : ITeachersRepository
{
    private readonly NpgsqlDataSource _source;
    public TeachersRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseTeacherDTO>> GetAllAsync()
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM teachers;
        """);
        using var reader = await command.ExecuteReaderAsync();
        List<ResponseTeacherDTO> teachers = new List<ResponseTeacherDTO>();
        while (await reader.ReadAsync())
        {
            teachers.Add(MapTeacher(reader));
        }
        return teachers;
    }
    public async Task<ResponseTeacherDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT * FROM teachers
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }
        return MapTeacher(reader);
    }
    public async Task<int> CreateAsync(CreateTeachersDTO teacher)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            INSERT INTO teachers(employee_code,full_name,email,phone,subject,hire_date,salary)
            VALUES(@employee_code,@full_name,@email,@phone,@subject,@hire_date,@salary)
            RETURNING id;
        """;
        command.Parameters.AddWithValue("@employee_code", teacher.EmployeeCode);
        command.Parameters.AddWithValue("@full_name", teacher.FullName);
        command.Parameters.AddWithValue("@email", teacher.Email);
        command.Parameters.AddWithValue("@phone", teacher.Phone);
        command.Parameters.AddWithValue("@subject", teacher.Subject);
        command.Parameters.AddWithValue("@hire_date", DateOnly.FromDateTime(DateTime.Now));
        command.Parameters.AddWithValue("@salary", teacher.Salary);
        int id = (int)(await command.ExecuteScalarAsync())!;
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateTeachersDTO teacher)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            UPDATE teachers
            SET
                employee_code = @employee_code,
                full_name = @full_name,
                email = @email,
                phone = @phone,
                subject = @subject,
                salary = @salary
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@employee_code", teacher.EmployeeCode);
        command.Parameters.AddWithValue("@full_name", teacher.FullName);
        command.Parameters.AddWithValue("@email", teacher.Email);
        command.Parameters.AddWithValue("@phone", teacher.Phone);
        command.Parameters.AddWithValue("@subject", teacher.Subject);
        command.Parameters.AddWithValue("@salary", teacher.Salary);
        command.Parameters.AddWithValue("@id", id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            DELETE FROM teachers
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    private ResponseTeacherDTO MapTeacher(NpgsqlDataReader reader)
    {
        return new ResponseTeacherDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            EmployeeCode = reader.GetInt32(reader.GetOrdinal("employee_code")),
            FullName = reader.GetString(reader.GetOrdinal("full_name")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            Phone = reader.GetString(reader.GetOrdinal("phone")),
            Subject = reader.GetString(reader.GetOrdinal("subject")),
            HireDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("hire_date"))),
            Salary = reader.GetDecimal(reader.GetOrdinal("salary"))
        };
    }
}