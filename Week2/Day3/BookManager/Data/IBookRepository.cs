using BookManager.Models;
namespace BookManager.Data;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<int> CreateAsync(Book book);
    Task<bool> UpdateAsync(int id, Book book);
    Task<bool> DeleteAsync(int id);
}