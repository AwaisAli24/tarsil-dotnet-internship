using BookManager.Models;
using Npgsql;

namespace BookManager.Data;

public class MemberRepository : IMemberRepository
{
    private readonly NpgsqlDataSource _dataSource;
    public MemberRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }
    public async Task<List<Member>> GetAllAsync()
    {
        List<Member> members = new List<Member>();
        using var command = _dataSource.CreateCommand("""
            SELECT * FROM members;
        """);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            members.Add(MapToObj(reader));
        }
        return members;

    }
    public async Task<Member?> GetByIdAsync(int id)
    {
        using var command = _dataSource.CreateCommand("""
            SELECT * FROM members
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReaderAsync();
        return MapToObj(await reader);
    }
    public async Task<int> CreateAsync(Member member)
    {
        using var command = _dataSource.CreateCommand("""
            INSERT INTO members(full_name,email,joined_on)
            VALUES(@full_name,@email,@joined_on)
            RETURNING id;
        """);
        command.Parameters.AddWithValue("@full_name", member.FullName);
        command.Parameters.AddWithValue("@email", member.Email);
        command.Parameters.AddWithValue("@joined_on", member.JoinedOn ?? DateOnly.FromDateTime(DateTime.Now));
        int id = Convert.ToInt32(await command.ExecuteScalarAsync());
        return id;
    }
    public async Task<bool> UpdateAsync(int id, Member member)
    {
        using var command = _dataSource.CreateCommand("""
            UPDATE TABLE members
            SET
            full_name = @full_name,
            email = @email,
            joined_on = @joined_on,
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@title", member.FullName);
        command.Parameters.AddWithValue("@isbn", member.Email);
        command.Parameters.AddWithValue("@price", member.JoinedOn ?? DateOnly.FromDateTime(DateTime.Now));
        command.Parameters.AddWithValue("@id", id);

        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _dataSource.CreateCommand("""
            DELETE FROM members
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    Member MapToObj(NpgsqlDataReader reader)
    {
        return new Member
        {
            Id = reader.GetInt32(0),
            FullName = reader.GetString(1),
            Email = reader.GetString(2),
            JoinedOn = DateOnly.FromDateTime(reader.GetDateTime(3))
        };
    }
}