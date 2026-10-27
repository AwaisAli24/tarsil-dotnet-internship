using InventoryApi.DTOs;
namespace InventoryApi.Services;

public interface ICategoryService
{
    Task<List<ResponseCategoryDTO>> GetCategoriesAsync();
    Task<ResponseCategoryDTO?> GetCategoryAsync(int id);
    Task<ResponseCategoryDTO> AddCategoryAsync(CreateCategoryDTO category);
    Task<ResponseCategoryDTO?> UpdateCategoryAsync(int id, UpdateCategoryDTO category);
    Task<ResponseCategoryDTO?> DeleteCategoryAsync(int id);
}