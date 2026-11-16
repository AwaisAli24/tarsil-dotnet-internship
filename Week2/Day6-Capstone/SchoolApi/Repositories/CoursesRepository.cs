using Npgsql;
using SchoolApi.DTOs.Courses;
using SchoolApi.DTOs.Students;
using SchoolApi.Repository.Interfaces;

namespace SchoolApi.Repository;

public class CoursesRepository : ICoursesRepository
{
    private readonly NpgsqlDataSource _source;
    public CoursesRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseCoursesDTO>> GetAllAsync()
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM courses;
        """);
        using var reader = await command.ExecuteReaderAsync();
        List<ResponseCoursesDTO> courses = new List<ResponseCoursesDTO>();
        while (await reader.ReadAsync())
        {
            courses.Add(MapCourse(reader));
        }
        return courses;
    }
    public async Task<ResponseCoursesDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT * FROM courses
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        await reader.ReadAsync();
        return MapCourse(reader);
    }
    public async Task<int> CreateAsync(CreateCoursesDTO course)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            INSERT INTO courses(code,title,credit_hours,capacity,teacher_id)
            VALUES(@code,@title,@credit_hours,@capacity,@teacher_id)
            RETURNING *;
        """;
        command.Parameters.AddWithValue("@code", course.Code);
        command.Parameters.AddWithValue("@title", course.Title);
        command.Parameters.AddWithValue("@credit_hours", course.CreditHours);
        command.Parameters.AddWithValue("@capacity", course.Capacity);
        command.Parameters.AddWithValue("@teacher_id", course.TeacherId);

        int id = (int)(await command.ExecuteScalarAsync())!;
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateCoursesDTO course)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            UPDATE courses
            SET
                code = @code,
                title = @title,
                credit_hours = @credit_hours,
                capacity = @capacity,
                teacher_id = @teacher_id
                WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@code", course.Code);
        command.Parameters.AddWithValue("@title", course.Title);
        command.Parameters.AddWithValue("@credit_hours", course.CreditHours);
        command.Parameters.AddWithValue("@capacity", course.Capacity);
        command.Parameters.AddWithValue("@teacher_id", course.TeacherId);
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            DELETE FROM courses
            WHERE id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<ResponseRosterDTO?> GetRosterAsync(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT c.code,c.title,c.credit_hours,c.capacity,t.full_name AS teacher_name,s.*,c.capacity - COUNT(e.course_id) over() AS seats_left FROM courses c
            LEFT JOIN enrollments e
            ON c.id = e.course_id
            LEFT JOIN teachers t
            ON t.id = c.teacher_id
            LEFT JOIN students s
            ON s.id = e.student_id
            WHERE c.id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        ResponseRosterDTO roster = new ResponseRosterDTO();

        using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        await reader.ReadAsync();
        roster.CourseCode = reader.GetString(reader.GetOrdinal("code"));
        roster.Title = reader.GetString(reader.GetOrdinal("title"));
        roster.CreditHours = reader.GetInt32(reader.GetOrdinal("credit_hours"));
        roster.Capacity = reader.GetInt32(reader.GetOrdinal("capacity"));
        roster.TeacherName = reader.GetString(reader.GetOrdinal("teacher_name"));
        roster.SeatsLeft = reader.GetInt32(reader.GetOrdinal("seats_left"));
        do
        {
            if (reader.IsDBNull(reader.GetOrdinal("id")))
            {
                break;
            }
            roster.EnrolledStudents.Add(StudentsRepository.MapStudent(reader));
        } while (await reader.ReadAsync());
        return roster;

    }
    private ResponseCoursesDTO MapCourse(NpgsqlDataReader reader)
    {
        return new ResponseCoursesDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Code = reader.GetString(reader.GetOrdinal("code")),
            Title = reader.GetString(reader.GetOrdinal("title")),
            CreditHours = reader.GetInt32(reader.GetOrdinal("credit_hours")),
            Capacity = reader.GetInt32(reader.GetOrdinal("capacity")),
            TeacherId = reader.GetInt32(reader.GetOrdinal("teacher_id"))
        };
    }
}