using BookManager.Models;
namespace BookManager.Data;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<int> CreateAsync(Book book);
    Task<bool> UpdateAsync(int id, Book book);
    Task<bool> DeleteAsync(int id);
    Task<List<Book>> SearchAsync(int page, int pageSize, string? title = null, int? authorId = null, decimal? minPrice = null, decimal? maxPrice = null);
}