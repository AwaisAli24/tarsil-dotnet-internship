using TodoApi.Models;

namespace TodoApi.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> AddAsync(Category category);
    Task<Category?> UpdateAsync(int id, Category category);
    Task<Category?> DeleteAsync(int id);
    Task<Category?> GetTodosAsync(int id);
}