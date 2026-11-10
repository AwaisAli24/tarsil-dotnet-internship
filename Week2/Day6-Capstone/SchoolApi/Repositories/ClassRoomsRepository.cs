using Npgsql;
using SchoolApi.DTO.ClassRoom;
using SchoolApi.Repository.Interfaces;

public class ClassRoomsRepository : IClassRoomsRepository
{
    private readonly NpgsqlDataSource _source;
    public ClassRoomsRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseClassRoomsDTO>> GetAllAsync()
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM class_rooms;
        """);
        using var reader = await command.ExecuteReaderAsync();
        List<ResponseClassRoomsDTO> classrooms = new List<ResponseClassRoomsDTO>();
        while (await reader.ReadAsync())
        {
            classrooms.Add(MapClassRoom(reader));
        }
        return classrooms;
    }
    public async Task<ResponseClassRoomsDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM class_rooms
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        await reader.ReadAsync();
        return MapClassRoom(reader);
    }
    public async Task<int> CreateAsync(CreateClassRoomsDTO classroom)
    {
        using var command = _source.CreateCommand("""
            INSERT INTO class_rooms(name,grade_level,section,academic_year)
            VALUES(@name,@grade_level,@section,@academic_year)
            RETURNING id;
        """);
        command.Parameters.AddWithValue("@name", classroom.Name);
        command.Parameters.AddWithValue("@grade_level", classroom.GradeLevel);
        command.Parameters.AddWithValue("@section", classroom.Section);
        command.Parameters.AddWithValue("@academic_year", classroom.AcademicYear);
        var id = (int)(await command.ExecuteScalarAsync())!;
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateClassRoomsDTO classroom)
    {
        using var command = _source.CreateCommand("""
            UPDATE class_rooms
            SET
                name = @name,
                grade_level = @grade_level,
                section = @section,
                academic_year = @academic_year
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@name", classroom.Name);
        command.Parameters.AddWithValue("@grade_level", classroom.GradeLevel);
        command.Parameters.AddWithValue("@section", classroom.Section);
        command.Parameters.AddWithValue("@academic_year", classroom.AcademicYear);
        command.Parameters.AddWithValue("@id", id);

        int affectedRows = await command.ExecuteNonQueryAsync();

        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand("""
            DELETE FROM class_rooms
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    private ResponseClassRoomsDTO MapClassRoom(NpgsqlDataReader reader)
    {
        return new ResponseClassRoomsDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            GradeLevel = reader.GetInt32(reader.GetOrdinal("grade_level")),
            Section = reader.GetString(reader.GetOrdinal("section")),
            AcademicYear = reader.GetInt32(reader.GetOrdinal("academic_year"))
        };
    }
}