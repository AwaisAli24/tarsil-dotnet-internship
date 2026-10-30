using BookManager.Models;
using Npgsql;
using NpgsqlTypes;

namespace BookManager.Data;

public class BookRepository : IBookRepository
{
    private readonly NpgsqlDataSource _dataSource;
    public BookRepository(NpgsqlDataSource datasource)
    {
        _dataSource = datasource;
    }
    public async Task<List<Book>> GetAllAsync()
    {
        List<Book> books = new List<Book>();
        using var command = _dataSource.CreateCommand("""
            SELECT * FROM books;
        """);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(MapToObj(reader));
        }
        return books;

    }
    public async Task<Book?> GetByIdAsync(int id)
    {
        using var command = _dataSource.CreateCommand("""
            SELECT * FROM books
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReaderAsync();
        return MapToObj(await reader);
    }
    public async Task<int> CreateAsync(Book book)
    {
        using var command = _dataSource.CreateCommand("""
            INSERT INTO books(title,isbn,price,total_copies,available_copies,published_year,author_id)
            VALUES(@title,@isbn,@price,@total_copies,@available_copies,@published_year,@author_id)
            RETURNING id;
        """);
        command.Parameters.AddWithValue("@title", book.Title);
        command.Parameters.AddWithValue("@isbn", book.Isbn);
        command.Parameters.AddWithValue("@price", book.Price);
        command.Parameters.AddWithValue("@total_copies", book.TotalCopies);
        command.Parameters.AddWithValue("@available_copies", book.AvailableCopies);
        command.Parameters.AddWithValue("@published_year", book.PublishedYear);
        command.Parameters.AddWithValue("@author_id", book.AuthorId);
        int id = -1;
        try
        {
            id = Convert.ToInt32(await command.ExecuteScalarAsync());

        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            Console.WriteLine("ISBN already exists.");

        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            Console.WriteLine("Author not Found");
        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, Book book)
    {
        using var command = _dataSource.CreateCommand("""
            UPDATE books
            SET
            title = @title,
            isbn = @isbn,
            price = @price,
            total_copies = @total_copies,
            available_copies = @available_copies,
            published_year = @published_year,
            author_id = @author_id
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@title", book.Title);
        command.Parameters.AddWithValue("@isbn", book.Isbn);
        command.Parameters.AddWithValue("@price", book.Price);
        command.Parameters.AddWithValue("@total_copies", book.TotalCopies);
        command.Parameters.AddWithValue("@available_copies", book.AvailableCopies);
        command.Parameters.AddWithValue("@published_year", book.PublishedYear);
        command.Parameters.AddWithValue("@author_id", book.AuthorId);
        command.Parameters.AddWithValue("@id", id);

        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var command = _dataSource.CreateCommand("""
            DELETE FROM books
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@id", id);
        var affectedRows = await command.ExecuteNonQueryAsync();
        return affectedRows == 1;
    }
    public async Task<List<Book>> SearchAsync(int page, int pageSize, string? title = null, int? authorId = null, decimal? minPrice = null, decimal? maxPrice = null)
    {
        if (page <= 0)
        {
            throw new ArgumentOutOfRangeException("Page number should be 1 or greater");
        }
        if (pageSize > 50 || pageSize < 1)
        {
            throw new ArgumentOutOfRangeException("Pagesize should be between 1 and 50");
        }
        var command = _dataSource.CreateCommand("""
            SELECT *
            FROM books
            WHERE (@title IS NULL OR title ILIKE '%' || @title || '%')
              AND (@authorId IS NULL OR author_id = @authorId)
              AND (@minPrice IS NULL OR price >= @minPrice)
              AND (@maxPrice IS NULL OR price <= @maxPrice)
            ORDER BY id ASC
            LIMIT @pageSize OFFSET @offset;
        """);

        command.Parameters.Add("@title", NpgsqlDbType.Text).Value =
    (object?)title ?? DBNull.Value;

        command.Parameters.Add("@authorId", NpgsqlDbType.Integer).Value =
            (object?)authorId ?? DBNull.Value;

        command.Parameters.Add("@minPrice", NpgsqlDbType.Numeric).Value =
            (object?)minPrice ?? DBNull.Value;

        command.Parameters.Add("@maxPrice", NpgsqlDbType.Numeric).Value =
            (object?)maxPrice ?? DBNull.Value;

        int offset = (page - 1) * pageSize;

        command.Parameters.Add("@pageSize", NpgsqlDbType.Integer).Value = pageSize;
        command.Parameters.Add("@offset", NpgsqlDbType.Integer).Value = offset;

        using var reader = await command.ExecuteReaderAsync();
        var books = new List<Book>();
        while (await reader.ReadAsync())
        {
            books.Add(MapToObj(reader));
        }

        return books;
    }
    Book MapToObj(NpgsqlDataReader reader)
    {
        return new Book
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Isbn = reader.GetString(2),
            Price = reader.GetDecimal(3),
            TotalCopies = reader.GetInt32(4),
            AvailableCopies = reader.GetInt32(5),
            PublishedYear = reader.GetInt32(6),
            AuthorId = reader.GetInt32(7)
        };
    }
}