using Npgsql;
using SchoolApi.DTO.Students;

public class StudentsRepository : IStudentsRepository
{
    private readonly NpgsqlDataSource _source;
    public StudentsRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseStudentsDTO>> GetAllAsync()
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT * FROM students;
        """;
        using var reader = await command.ExecuteReaderAsync();
        List<ResponseStudentsDTO> students = new List<ResponseStudentsDTO>();
        while (await reader.ReadAsync())
        {
            students.Add(MapStudent(reader));
        }
        return students;
    }
    public async Task<ResponseStudentsDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT * FROM students
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }
        return MapStudent(reader);
    }
    public async Task<int> CreateAsync(CreateStudentsDTO student)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            INSERT INTO students(roll_number,first_name,last_name,email,class_room_id,enrolled_on)
            VALUES(@roll_number,@first_name,@last_name,@email,@class_room_id,@enrolled_on)
            RETURNING id;
        """;
        command.Parameters.AddWithValue("@roll_number", student.RollNumber);
        command.Parameters.AddWithValue("@first_name", student.FirstName);
        command.Parameters.AddWithValue("@last_name", student.LastName);
        command.Parameters.AddWithValue("@email", student.Email);
        command.Parameters.AddWithValue("@class_room_id", student.ClassRoomId);
        command.Parameters.AddWithValue("enrolled_on", DateOnly.FromDateTime(DateTime.Now));

        int id = (int)(await command.ExecuteScalarAsync())!;
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateStudentsDTO student)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            UPDATE students
            SET
                roll_number = @roll_number,
                first_name = @first_name,
                last_name = @last_name,
                email = @email,
                class_room_id = @class_room_id
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@roll_number", student.RollNumber);
        command.Parameters.AddWithValue("@first_name", student.FirstName);
        command.Parameters.AddWithValue("@last_name", student.LastName);
        command.Parameters.AddWithValue("@email", student.Email);
        command.Parameters.AddWithValue("@class_room_id", student.ClassRoomId);
        command.Parameters.AddWithValue("@id", id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            DELETE FROM students
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    private ResponseStudentsDTO MapStudent(NpgsqlDataReader reader)
    {
        return new ResponseStudentsDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            RollNumber = reader.GetString(reader.GetOrdinal("roll_number")),
            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
            LastName = reader.GetString(reader.GetOrdinal("last_name")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            ClassRoomId = reader.GetInt32(reader.GetOrdinal("class_room_id")),
            Enrolled_on = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("enrolled_on")))
        };
    }
}