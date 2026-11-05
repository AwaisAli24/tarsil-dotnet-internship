using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories.Interfaces;
using Npgsql;
using NpgsqlTypes;

namespace LibraryApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly NpgsqlDataSource _source;
    public BookRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseBookDTO>> GetAllAsync(int page, int pageSize, string? title = null, int? authorId = null, decimal? minPrice = null, decimal? maxPrice = null)
    {
        using var command = _source.CreateCommand("""
            SELECT * FROM books
            WHERE (@title IS NULL OR title ILIKE '%' || @title || '%')
            AND (@authorId IS NULL OR author_id = @authorId)
            AND (@minPrice IS NULL OR price >= @minPrice)
            AND (@maxPrice IS NULL OR price <= @maxPrice)
            ORDER BY id
            LIMIT @pageSize OFFSET @offset;
        """);
        command.Parameters.AddWithValue("@pageSize", pageSize);
        command.Parameters.Add("@title", NpgsqlDbType.Text).Value =
    (object?)title ?? DBNull.Value;
        command.Parameters.Add("@authorId", NpgsqlDbType.Integer).Value =
    (object?)authorId ?? DBNull.Value;
        command.Parameters.Add("@minPrice", NpgsqlDbType.Numeric).Value =
    (object?)minPrice ?? DBNull.Value;
        command.Parameters.Add("@maxPrice", NpgsqlDbType.Numeric).Value =
    (object?)maxPrice ?? DBNull.Value;
        command.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
        var reader = await command.ExecuteReaderAsync();

        List<ResponseBookDTO> books = new List<ResponseBookDTO>();
        while (await reader.ReadAsync())
        {
            books.Add(MapToDTO(reader));
        }
        return books;
    }
    public async Task<ResponseBookDTO?> GetByIdAsync(int id)
    {
        using var command = _source.CreateCommand("""
            SELECT b.id as book_id, b.title, b.isbn, b.price, b.total_copies, b.available_copies, b.published_year, b.author_id FROM books b
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (reader.HasRows == false)
        {
            return null;
        }
        await reader.ReadAsync();
        return MapToDTO(reader);
    }
    public async Task<ResponseBookDTO> CreateAsync(CreateBookDTO book)
    {
        using var command = _source.CreateCommand("""
            INSERT INTO books(title,isbn,price,total_copies,available_copies,published_year,author_id)
            VALUES(@title,@isbn,@price,@total_copies,@available_copies,@published_year,@author_id)
            RETURNING *;
        """);
        command.Parameters.AddWithValue("title", book.Title);
        command.Parameters.AddWithValue("isbn", book.Isbn);
        command.Parameters.AddWithValue("price", book.Price);
        command.Parameters.AddWithValue("total_copies", book.TotalCopies);
        command.Parameters.AddWithValue("available_copies", book.AvailableCopies);
        command.Parameters.AddWithValue("published_year", book.PublishedYear);
        command.Parameters.AddWithValue("author_id", book.AuthorId);
        using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return MapToDTO(reader);
    }
    public async Task<ResponseBookDTO?> UpdateAsync(int id, UpdateBookDTO book)
    {
        using var command = _source.CreateCommand("""
            UPDATE books
            SET
            title = @title,
            isbn = @isbn,
            price = @price,
            total_copies = @total_copies,
            available_copies = @available_copies,
            published_year = @published_year,
            author_id = @author_id
            WHERE id = @id
            RETURNING *;
        """);
        command.Parameters.AddWithValue("title", book.Title);
        command.Parameters.AddWithValue("isbn", book.Isbn);
        command.Parameters.AddWithValue("price", book.Price);
        command.Parameters.AddWithValue("total_copies", book.TotalCopies);
        command.Parameters.AddWithValue("available_copies", book.AvailableCopies);
        command.Parameters.AddWithValue("published_year", book.PublishedYear);
        command.Parameters.AddWithValue("author_id", book.AuthorId);
        command.Parameters.AddWithValue("id", id);
        using var reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }
        await reader.ReadAsync();
        var toUpdate = MapToDTO(reader);
        return toUpdate;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _source.CreateCommand("""
            DELETE FROM books
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }
    public static ResponseBookDTO MapToDTO(NpgsqlDataReader reader)
    {
        return new ResponseBookDTO
        {
            Id = reader.GetInt32(reader.GetOrdinal("book_id")),
            Title = reader.GetString(reader.GetOrdinal("title")),
            Isbn = reader.GetString(reader.GetOrdinal("isbn")),
            Price = reader.GetDecimal(reader.GetOrdinal("price")),
            TotalCopies = reader.GetInt32(reader.GetOrdinal("total_copies")),
            AvailableCopies = reader.GetInt32(reader.GetOrdinal("available_copies")),
            PublishedYear = reader.GetInt32(reader.GetOrdinal("published_year")),
            AuthorId = reader.GetInt32(reader.GetOrdinal("author_id"))
        };
    }
}