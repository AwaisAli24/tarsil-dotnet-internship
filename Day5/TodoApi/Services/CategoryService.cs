using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;

class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Category>> GetAsync()
    {
        var category = await _context.Categories.ToListAsync();
        return category;
    }
    public async Task<Category?> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return category;
        }
        return category;
    }
    public async Task<Category> AddAsync(Category category)
    {
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }
    public async Task<Category?> UpdateAsync(int id, Category category)
    {
        var toUpdate = await _context.Categories.FindAsync(id);
        if (toUpdate == null)
        {
            return toUpdate;
        }
        toUpdate.Name = category.Name;
        toUpdate.Todos = category.Todos;
        return toUpdate;
    }
    public async Task<Category?> DeleteAsync(int id)
    {
        var toDelete = await _context.Categories.FindAsync(id);
        if (toDelete == null)
        {
            return toDelete;
        }
        _context.Remove(toDelete);
        await _context.SaveChangesAsync();
        return toDelete;
    }
    public async Task<Category?> GetTodosAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        category.Todos = await _context.Todos.Where(t => t.CategoryId == id).ToListAsync();
        return category;
    }
}