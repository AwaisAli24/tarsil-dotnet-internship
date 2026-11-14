
using Npgsql;
using SchoolApi.DTOs.Enrollments;
using SchoolApi.Repository.Interfaces;

namespace SchoolApi.Repository;

public class EnrollmentsRepository : IEnrollmentRepository
{
    private readonly NpgsqlDataSource _source;
    public EnrollmentsRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<int> AddEnrollmentAsync(CreateEnrollmentsDTO enrollment)
    {
        using var connection = _source.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = """
                SELECT capacity FROM courses
                WHERE id = @course_id
                FOR UPDATE;
            """;
            command.Parameters.AddWithValue("@course_id", enrollment.CourseId);
            var result = await command.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Course Does not exist.");
            }
            int capacity = Convert.ToInt32(result);

            command.CommandText = """
                SELECT COUNT(*) FROM enrollments
                WHERE course_id = @course_id;
            """;
            command.Parameters.AddWithValue("@course_id", enrollment.CourseId);
            long currentlyEnrolled = (long)(await command.ExecuteScalarAsync())!;

            if (currentlyEnrolled + 1 > capacity)
            {
                throw new InvalidOperationException("Capacity Exceeded");
            }

            command.CommandText = """
                SELECT EXISTS(
                    SELECT * FROM enrollments
                    WHERE course_id = @course_id and student_id = @student_id
                );
            """;
            command.Parameters.AddWithValue("@course_id", enrollment.CourseId);
            command.Parameters.AddWithValue("@student_id", enrollment.StudentId);
            bool exists = (bool)(await command.ExecuteScalarAsync())!;

            if (exists)
            {
                throw new InvalidOperationException("Student is already enrolled in the course.");
            }

            command.CommandText = """
                INSERT INTO enrollments(student_id,course_id,enrolled_on)
                VALUES(@student_id,@course_id,@enrolled_on)
                RETURNING id;
            """;
            command.Parameters.AddWithValue("@student_id", enrollment.StudentId);
            command.Parameters.AddWithValue("@course_id", enrollment.CourseId);
            command.Parameters.AddWithValue("@enrolled_on", DateOnly.FromDateTime(DateTime.Now));

            int id = (int)(await command.ExecuteScalarAsync())!;
            await transaction.CommitAsync();
            return id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<bool> UpdateMarksAsync(int id, int marks)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            UPDATE enrollments
            SET
                marks = @marks,
                grade = @grade
            WHERE id = @id;
        """;
        string grade = SchoolApi.Services.GradingRules.CalculateGrade(marks);
        command.Parameters.AddWithValue("@marks", marks);
        command.Parameters.AddWithValue("@grade", grade);
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
}