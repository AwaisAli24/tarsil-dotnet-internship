using Npgsql;
using SchoolApi.DTOs.Reports;
using SchoolApi.Repositories.Interfaces;

namespace SchoolApi.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly NpgsqlDataSource _source;
    public ReportsRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseToppersDTO>> GetToppersAsync(int id, int limit)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT s.id,s.first_name,COALESCE(e.marks,0) as marks,COALESCE(e.grade,'') as grade FROM students s
            JOIN enrollments e
            ON s.id=e.student_id
            WHERE s.class_room_id = @id
            ORDER BY marks DESC
            LIMIT @limit;
        """;
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@limit", limit);
        List<ResponseToppersDTO> toppers = new List<ResponseToppersDTO>();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            toppers.Add(new ResponseToppersDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("first_name")),
                Marks = reader.GetInt32(reader.GetOrdinal("marks")),
                Grade = reader.GetString(reader.GetOrdinal("grade"))
            });
        }
        return toppers;
    }
    public async Task<ResponseStatsDTO?> GetCourseStats(int id)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT EXISTS(
                SELECT 1 FROM courses
                WHERE id = @id
            );
        """;
        command.Parameters.AddWithValue("@id", id);
        bool exists = (bool)(await command.ExecuteScalarAsync())!;
        if (!exists)
        {
            return null;
        }

        command.CommandText = """
            SELECT COALESCE(AVG(marks),0) AS average_marks,COALESCE(MAX(marks),0) AS highest_marks,COALESCE(MIN(marks),0) AS lowest_marks
            FROM enrollments
            WHERE course_id = @id;
        """;
        command.Parameters.AddWithValue("@id", id);
        ResponseStatsDTO stats;
        {

            using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            stats = new ResponseStatsDTO
            {
                CourseId = id,
                AvgMarks = reader.GetDouble(reader.GetOrdinal("average_marks")),
                HighestMarks = reader.GetInt32(reader.GetOrdinal("highest_marks")),
                LowestMarks = reader.GetInt32(reader.GetOrdinal("lowest_marks"))
            };
        }
        command.CommandText = """
            SELECT grade,count(grade) AS count FROM enrollments
            WHERE course_id = @id
            GROUP BY grade
            ORDER BY grade;
        """;
        command.Parameters.AddWithValue("@id", id);
        using var gradeReader = await command.ExecuteReaderAsync();
        if (!gradeReader.HasRows)
        {
            return stats;
        }
        while (await gradeReader.ReadAsync())
        {
            stats.GradeCounts.Add(new GradeCount
            {
                Grade = gradeReader.GetString(gradeReader.GetOrdinal("grade")),
                Count = gradeReader.GetInt32(gradeReader.GetOrdinal("count"))
            });
        }
        return stats;
    }
}