using Npgsql;
using NpgsqlTypes;
using SchoolApi.DTOs.Courses;
using SchoolApi.DTOs.Students;

public class StudentsRepository : IStudentsRepository
{
    private readonly NpgsqlDataSource _source;
    public StudentsRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseStudentsDTO>> GetAllAsync(int limit, int offset, int? classRoomId, string? search)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT * FROM students
            WHERE (@classRoomId IS NULL OR class_room_id = @classRoomId) AND
                (@search IS NULL OR first_name ILIKE '%' || @search || '%' OR last_name ILIKE '%' || @search || '%')
                ORDER BY id
                limit @limit offset @offset;
        """;
        command.Parameters.Add("@classRoomId", NpgsqlDbType.Integer).Value = (object?)classRoomId ?? DBNull.Value;
        command.Parameters.Add("@search", NpgsqlDbType.Text).Value = (object?)search ?? DBNull.Value;
        command.Parameters.AddWithValue("@limit", limit);
        command.Parameters.AddWithValue("@offset", offset);
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
            INSERT INTO students(roll_number,first_name,last_name,email,class_room_id,enrolled_on,date_of_birth)
            VALUES(@roll_number,@first_name,@last_name,@email,@class_room_id,@enrolled_on,@date_of_birth)
            RETURNING id;
        """;
        command.Parameters.AddWithValue("@roll_number", student.RollNumber);
        command.Parameters.AddWithValue("@first_name", student.FirstName);
        command.Parameters.AddWithValue("@last_name", student.LastName);
        command.Parameters.AddWithValue("@email", student.Email);
        command.Parameters.AddWithValue("@class_room_id", student.ClassRoomId);
        command.Parameters.AddWithValue("@enrolled_on", DateOnly.FromDateTime(DateTime.Now));
        command.Parameters.AddWithValue("@date_of_birth", student.DateOfBirth);

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
                class_room_id = @class_room_id,
                date_of_birth = @date_of_birth
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@roll_number", student.RollNumber);
        command.Parameters.AddWithValue("@first_name", student.FirstName);
        command.Parameters.AddWithValue("@last_name", student.LastName);
        command.Parameters.AddWithValue("@email", student.Email);
        command.Parameters.AddWithValue("@class_room_id", student.ClassRoomId);
        command.Parameters.AddWithValue("@date_of_birth", student.DateOfBirth);
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
        int affectedRows;
        try
        {

            affectedRows = await command.ExecuteNonQueryAsync();
        }
        catch (PostgresException e) when (e.SqlState == "23503" || e.SqlState == "23001")
        {
            throw new InvalidOperationException("Cannot delete student because it is referenced in the enrollments table.");
        }
        return affectedRows == 1;
    }
    public async Task<ResponseTranscriptDTO?> GetTranscriptAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT marks FROM enrollments
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        var exist = await command.ExecuteScalarAsync();
        command.CommandText = """
            SELECT s.first_name, s.last_name, t.full_name AS teacher_name, c.title AS course_title ,e.marks, e.grade, COALESCE(AVG(e.marks)OVER(PARTITION BY e.student_id),0) as avg FROM students s
            LEFT JOIN enrollments e
            ON s.id=e.student_id
            LEFT JOIN courses c
            ON e.course_id = c.id
            LEFT JOIN teachers t
            ON t.id = c.teacher_id
            WHERE s.id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        ResponseTranscriptDTO transcript = new ResponseTranscriptDTO();
        await reader.ReadAsync();
        transcript.FirstName = reader.GetString(reader.GetOrdinal("first_name"));
        transcript.LastName = reader.GetString(reader.GetOrdinal("last_name"));
        transcript.AvgMarks = reader.GetDouble(reader.GetOrdinal("avg"));
        do
        {
            if (reader.IsDBNull(reader.GetOrdinal("course_title")))
            {
                break;
            }
            if (reader.IsDBNull(reader.GetOrdinal("marks")))
            {
                transcript.InProgressCourses.Add(new InProgressTranscriptCoursesDTO
                {
                    TeacherName = reader.GetString(reader.GetOrdinal("teacher_name")),
                    CourseTitle = reader.GetString(reader.GetOrdinal("course_title"))
                });
                continue;
            }
            transcript.CompletedCourses.Add(new CompletedTranscriptCoursesDTO
            {
                TeacherName = reader.GetString(reader.GetOrdinal("teacher_name")),
                CourseTitle = reader.GetString(reader.GetOrdinal("course_title")),
                Marks = reader.GetInt32(reader.GetOrdinal("marks")),
                Grade = reader.GetString(reader.GetOrdinal("grade"))
            });
        } while (await reader.ReadAsync());
        return transcript;
    }

    public async Task<bool> ImportStudentDataCSVAsync(List<ImportStudentsDTO> students)
    {
        using var connection = _source.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = """
                INSERT INTO students(roll_number,first_name,last_name,email,class_room_id,enrolled_on)
                VALUES(@roll_number,@first_name,@last_name,@email,@class_room_id,@enrolled_on);
            """;
            foreach (var student in students)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@roll_number", student.RollNumber);
                command.Parameters.AddWithValue("@first_name", student.FirstName);
                command.Parameters.AddWithValue("@last_name", student.LastName);
                command.Parameters.AddWithValue("@email", student.Email);
                command.Parameters.AddWithValue("@class_room_id", student.ClassRoomId);
                command.Parameters.AddWithValue("enrolled_on", DateOnly.FromDateTime(DateTime.Now));
                await command.ExecuteNonQueryAsync();
            }
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public static ResponseStudentsDTO MapStudent(NpgsqlDataReader reader)
    {
        return new ResponseStudentsDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            RollNumber = reader.GetString(reader.GetOrdinal("roll_number")),
            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
            LastName = reader.GetString(reader.GetOrdinal("last_name")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            ClassRoomId = reader.GetInt32(reader.GetOrdinal("class_room_id")),
            Enrolled_on = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("enrolled_on"))),
            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth")))
        };
    }

}