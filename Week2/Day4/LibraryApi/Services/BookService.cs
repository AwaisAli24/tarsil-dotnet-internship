using LibraryApi.Services.Interfaces;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.DTOs;
using Npgsql;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseBookDTO>> GetAllAsync(int page, int pageSize, string? title = null, int? authorId = null, decimal? minPrice = null, decimal? maxPrice = null)
    {
        return await _repository.GetAllAsync(page, pageSize, title, authorId, minPrice, maxPrice);
    }
    public async Task<ResponseBookDTO?> GetByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null)
        {
            return null;
        }
        return book;
    }
    public async Task<ResponseBookDTO> CreateAsync(CreateBookDTO book)
    {
        if (book.AvailableCopies > book.TotalCopies)
        {
            throw new BadHttpRequestException("Available copies should be less or equal than the total copies.");
        }
        if (book.Price <= 0)
        {
            throw new BadHttpRequestException("Price should be greater than 0.");
        }
        ResponseBookDTO toAdd;
        try
        {
            toAdd = await _repository.CreateAsync(book);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new ArgumentException("ISBN already Exist");
        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            throw new ArgumentException("Author Id does not exist");
        }
        return toAdd;
    }
    public async Task<ResponseBookDTO?> UpdateAsync(int id, UpdateBookDTO book)
    {
        if (book.AvailableCopies > book.TotalCopies)
        {
            throw new BadHttpRequestException("Available copies should be less or equal than the total copies.");
        }
        if (book.Price <= 0)
        {
            throw new BadHttpRequestException("Price should be greater than 0.");
        }
        ResponseBookDTO? toUpdate;
        try
        {
            toUpdate = await _repository.UpdateAsync(id, book);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new ArgumentException("ISBN already Exist");
        }
        catch (PostgresException e) when (e.SqlState == "23503")
        {
            throw new ArgumentException("Author Id does not exist");
        }
        return toUpdate;
    }
    public async Task<bool> DeleteAsync(int id)
    {

        var toDelete = await _repository.DeleteAsync(id);
        return toDelete;
    }
}