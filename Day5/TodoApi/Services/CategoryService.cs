using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Dto;

class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<CategoryResponseDto>> GetAsync()
    {
        var category = await _context.Categories.ToListAsync();
        List<CategoryResponseDto> response = category.Select(c => new CategoryResponseDto { Name = c.Name, Id = c.Id }).ToList();
        return response;
    }
    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return null;
        }
        return new CategoryResponseDto { Name = category.Name, Id = category.Id };
    }
    public async Task<CategoryResponseDto> AddAsync(CreateCategoryDto categoryDto)
    {
        Category category = new Category
        {
            Name = categoryDto.Name
        };
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();
        return new CategoryResponseDto { Name = categoryDto.Name };
    }
    public async Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto categoryDto)
    {
        var toUpdate = await _context.Categories.FindAsync(id);
        if (toUpdate == null)
        {
            return null;
        }
        toUpdate.Name = categoryDto.Name;
        await _context.SaveChangesAsync();
        return new CategoryResponseDto { Name = categoryDto.Name };
    }
    public async Task<CategoryResponseDto?> DeleteAsync(int id)
    {
        var toDelete = await _context.Categories.FindAsync(id);
        if (toDelete == null)
        {
            return null;
        }
        try
        {
            _context.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Cannot delete category because it has associated todos.");
        }
        return new CategoryResponseDto { Name = toDelete.Name };
    }
    public async Task<TodosByCategoryDto?> GetTodosAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return null;
        }
        var todos = await _context.Todos.Where(t => t.CategoryId == id).ToListAsync();
        // category.Todos = category.Todos.Select(TodoService.MapToDTO).ToList();
        // return category;
        TodosByCategoryDto Todos = new TodosByCategoryDto
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
            Todos = todos.Select(TodoService.MapToDTO).ToList()
        };
        return Todos;

    }
}