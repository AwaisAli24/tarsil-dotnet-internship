using LibraryApi.DTOs;
using Npgsql;
using LibraryApi.Repositories.Interfaces;

namespace LibraryApi.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly NpgsqlDataSource _source;
    public MemberRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseMemberDTO>> GetAllAsync()
    {
        using var command = _source.CreateCommand("""
        SELECT * FROM members;
        """);
        List<ResponseMemberDTO> members = new List<ResponseMemberDTO>();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            members.Add(MapToDTO(reader));
        }
        return members;
    }
    public async Task<ResponseMemberDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM members
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        await reader.ReadAsync();
        return MapToDTO(reader);
    }
    public async Task<int> CreateAsync(CreateMemberDTO member)
    {
        using var command = _source.CreateCommand("""
            INSERT INTO members(full_name,email,joined_on)
            VALUES(@full_name,@email,@joined_on)
            RETURNING id;
        """);
        command.Parameters.AddWithValue("@full_name", member.FullName);
        command.Parameters.AddWithValue("@email", member.Email);
        command.Parameters.AddWithValue("joined_on", member.JoinedOn);

        var id = await command.ExecuteScalarAsync();
        return Convert.ToInt32(id);
    }
    public async Task<bool> UpdateAsync(int id, UpdateMemberDTO member)
    {
        using var command = _source.CreateCommand("""
            UPDATE members
            SET full_name = @full_name,
            email = @email
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@full_name", member.FullName);
        command.Parameters.AddWithValue("@email", member.Email);
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand("""
            DELETE FROM members
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        int affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    // public async Task<List<ResponseLoanDTO>> GetLoansByMemberAsync(int id,bool? isActive)
    // {
    //     using var command = _source.CreateCommand("""

    //     """);
    // }
    private ResponseMemberDTO MapToDTO(NpgsqlDataReader reader)
    {
        return new ResponseMemberDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            FullName = reader.GetString(reader.GetOrdinal("full_name")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            JoinedOn = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("joined_on")))
        };
    }
}