using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<ResponseCategoryDTO>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        var categoriesDtoList = categories.Select(MapToDto).ToList();
        return categoriesDtoList;
    }
    public async Task<ResponseCategoryDTO?> GetCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return null;
        }
        return MapToDto(category);
    }
    public async Task<ResponseCategoryDTO> AddCategoryAsync(CreateCategoryDTO category)
    {
        Category toAdd = new Category
        {
            Name = category.Name,
            Description = category.Description
        };
        await _context.Categories.AddAsync(toAdd);
        await _context.SaveChangesAsync();
        return MapToDto(toAdd);
    }
    public async Task<ResponseCategoryDTO?> UpdateCategoryAsync(int id, UpdateCategoryDTO category)
    {
        Category? toUpdate = await _context.Categories.FindAsync(id);
        if (toUpdate == null)
        {
            return null;
        }
        toUpdate.Name = category.Name;
        toUpdate.Description = category.Description;
        await _context.SaveChangesAsync();
        return MapToDto(toUpdate);
    }
    public async Task<ResponseCategoryDTO?> DeleteCategoryAsync(int id)
    {
        Category? toDelete = await _context.Categories.FindAsync(id);
        if (toDelete == null)
        {
            return null;
        }
        bool isExist = await _context.Products.AnyAsync(p => p.CategoryId == id);
        if (isExist)
        {
            throw new InvalidOperationException("This Category is associated with a product. Cannot be Deleted");
        }
        _context.Categories.Remove(toDelete);
        await _context.SaveChangesAsync();
        return MapToDto(toDelete);
    }
    public static ResponseCategoryDTO MapToDto(Category category)
    {
        return new ResponseCategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
}