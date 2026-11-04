using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
using Npgsql;

namespace LibraryApi.Repositories;

class AuthorRepository : IAuthorRepository
{
    private readonly NpgsqlDataSource _source;
    public AuthorRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseAuthorDTO>> GetAllAsync()
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM authors;
        """);
        using var reader = await command.ExecuteReaderAsync();
        List<ResponseAuthorDTO> authors = new List<ResponseAuthorDTO>();
        while (await reader.ReadAsync())
        {
            authors.Add(MaptoDTO(reader));
        }
        return authors;
    }
    public async Task<ResponseAuthorDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM authors
            WHERE id = @id
        """);
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (reader.HasRows == false)
        {
            return null;
        }
        await reader.ReadAsync();
        return MaptoDTO(reader);
    }
    public async Task<ResponseAuthorDTO> CreateAsync(CreateAuthorDTO author)
    {
        using var command = _source.CreateCommand("""
            INSERT INTO authors(name,country)
            VALUES(@name,@country)
            RETURNING *;
        """);
        command.Parameters.AddWithValue("@name", author.Name);
        command.Parameters.AddWithValue("@country", author.Country);
        using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return MaptoDTO(reader);
    }
    public async Task<ResponseAuthorDTO> UpdateAsync(int id, UpdateAuthorDTO author)
    {
        using var command = _source.CreateCommand("""
            UPDATE authors
            SET 
            name = @name,
            country = @country
            WHERE id = @id
            RETURNING *;
        """);
        command.Parameters.AddWithValue("@name", author.Name);
        command.Parameters.AddWithValue("@country", author.Country);
        command.Parameters.AddWithValue("@id", id);
        using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return MaptoDTO(reader);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand("""
            DELETE FROM authors
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
    private ResponseAuthorDTO MaptoDTO(NpgsqlDataReader reader)
    {
        return new ResponseAuthorDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Country = reader.GetString(reader.GetOrdinal("country"))
        };
    }
}