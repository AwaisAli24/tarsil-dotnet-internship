using BookManager.Models;
namespace BookManager.Data;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<int> CreateAsync(Author author);
    Task<bool> UpdateAsync(int id, Author author);
    Task<bool> DeleteAsync(int id);
}