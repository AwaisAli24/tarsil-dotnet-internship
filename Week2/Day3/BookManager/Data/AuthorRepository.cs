using BookManager.Models;
using Npgsql;

namespace BookManager.Data;

public class AuthorRepository : IAuthorRepository
{
    private readonly NpgsqlDataSource _dataSource;
    public AuthorRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }
    public async Task<List<Author>> GetAllAsync()
    {
        List<Author> authors = new List<Author>();
        using var command = _dataSource.CreateCommand("""
            SELECT * FROM author;
        """);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            authors.Add(MapToObj(reader));
        }
        return authors;

    }
    public async Task<Author?> GetByIdAsync(int id)
    {
        using var command = _dataSource.CreateCommand("""
            SELECT * FROM author
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReaderAsync();
        return MapToObj(await reader);
    }
    public async Task<int> CreateAsync(Author author)
    {
        using var command = _dataSource.CreateCommand("""
            INSERT INTO author(name,country)
            VALUES(@name,@country)
            RETURNING id;
        """);
        command.Parameters.AddWithValue("@title", author.Name);
        command.Parameters.AddWithValue("@isbn", author.Country);
        int id = Convert.ToInt32(await command.ExecuteScalarAsync());
        return id;
    }
    public async Task<bool> UpdateAsync(int id, Author author)
    {
        using var command = _dataSource.CreateCommand("""
            UPDATE TABLE author
            SET
            name = @name,
            country = @country,
        """);
        command.Parameters.AddWithValue("@name", author.Name);
        command.Parameters.AddWithValue("@country", author.Country);
        command.Parameters.AddWithValue("@id", id);

        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _dataSource.CreateCommand("""
            DELETE FROM author
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    Author MapToObj(NpgsqlDataReader reader)
    {
        return new Author
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Country = reader.GetString(2)
        };
    }
}