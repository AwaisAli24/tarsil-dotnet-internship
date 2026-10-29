using BookManager.Models;
using Npgsql;

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
            INSERT INTO books(title,isbn,price,total_copies,available_copies,published_year)
            VALUES(@title,@isbn,@price,@total_copies,@available_copies,@published_year)
            RETURNING id;
        """);
        command.Parameters.AddWithValue("@title", book.Title);
        command.Parameters.AddWithValue("@isbn", book.Isbn);
        command.Parameters.AddWithValue("@price", book.Price);
        command.Parameters.AddWithValue("@total_copies", book.TotalCopies);
        command.Parameters.AddWithValue("@available_copies", book.AvailableCopies);
        command.Parameters.AddWithValue("@published_year", book.PublishedYear);
        int id = Convert.ToInt32(await command.ExecuteScalarAsync());
        return id;
    }
    public async Task<bool> UpdateAsync(int id, Book book)
    {
        using var command = _dataSource.CreateCommand("""
            UPDATE TABLE books
            SET
            title = @title,
            isbn = @isbn,
            price = @price,
            total_copies = @total_copies,
            available_copies = @available_copies,
            published_year = @published_year
            WHERE id = @id;
        """);
        command.Parameters.AddWithValue("@title", book.Title);
        command.Parameters.AddWithValue("@isbn", book.Isbn);
        command.Parameters.AddWithValue("@price", book.Price);
        command.Parameters.AddWithValue("@total_copies", book.TotalCopies);
        command.Parameters.AddWithValue("@available_copies", book.AvailableCopies);
        command.Parameters.AddWithValue("@published_year", book.PublishedYear);
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
            PublishedYear = reader.GetInt32(6)
        };
    }
}