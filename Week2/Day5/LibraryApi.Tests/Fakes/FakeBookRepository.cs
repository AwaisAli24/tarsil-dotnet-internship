using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
using Npgsql;

namespace LibraryApi.Tests.Fakes;

public class FakeBookRepository : IBookRepository
{
    public List<ResponseBookDTO> Books { get; } = new();
    public HashSet<int> AuthorIds { get; } = new();
    private int _nextId = 1;

    public void AddAuthor(int authorId)
    {
        AuthorIds.Add(authorId);
    }

    public ResponseBookDTO AddExistingBook(ResponseBookDTO book)
    {
        Books.Add(book);
        if (book.Id >= _nextId)
        {
            _nextId = book.Id + 1;
        }
        return book;
    }

    public Task<List<ResponseBookDTO>> GetAllAsync(
        int page,
        int pageSize,
        string? title = null,
        int? authorId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var query = Books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(b => b.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(b => b.Price <= maxPrice.Value);
        }

        var paged = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(paged);
    }

    public Task<ResponseBookDTO?> GetByIdAsync(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    public Task<ResponseBookDTO> CreateAsync(CreateBookDTO book)
    {
        // Simulate foreign key check if authors are tracked
        if (AuthorIds.Count > 0 && !AuthorIds.Contains(book.AuthorId))
        {
            throw new PostgresException("foreign key violation", "ERROR", "ERROR", "23503");
        }

        // Simulate unique constraint on ISBN
        if (Books.Any(b => b.Isbn == book.Isbn))
        {
            throw new PostgresException("unique_violation", "ERROR", "ERROR", "23505");
        }

        var created = new ResponseBookDTO
        {
            Id = _nextId++,
            Title = book.Title,
            Isbn = book.Isbn,
            Price = book.Price,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            PublishedYear = book.PublishedYear,
            AuthorId = book.AuthorId
        };

        Books.Add(created);
        return Task.FromResult(created);
    }

    public Task<ResponseBookDTO?> UpdateAsync(int id, UpdateBookDTO book)
    {
        var existing = Books.FirstOrDefault(b => b.Id == id);
        if (existing == null)
        {
            return Task.FromResult<ResponseBookDTO?>(null);
        }

        // Simulate foreign key check if authors are tracked
        if (AuthorIds.Count > 0 && !AuthorIds.Contains(book.AuthorId))
        {
            throw new PostgresException("foreign key violation", "ERROR", "ERROR", "23503");
        }

        // Simulate unique constraint on ISBN when conflicting with other books
        if (Books.Any(b => b.Id != id && b.Isbn == book.Isbn))
        {
            throw new PostgresException("unique_violation", "ERROR", "ERROR", "23505");
        }

        existing.Title = book.Title;
        existing.Isbn = book.Isbn;
        existing.Price = book.Price;
        existing.TotalCopies = book.TotalCopies;
        existing.AvailableCopies = book.AvailableCopies;
        existing.PublishedYear = book.PublishedYear;
        existing.AuthorId = book.AuthorId;

        return Task.FromResult<ResponseBookDTO?>(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var existing = Books.FirstOrDefault(b => b.Id == id);
        if (existing == null)
        {
            return Task.FromResult(false);
        }

        Books.Remove(existing);
        return Task.FromResult(true);
    }
}

