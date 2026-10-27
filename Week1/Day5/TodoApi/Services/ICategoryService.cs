using TodoApi.Models;
using TodoApi.Dto;
namespace TodoApi.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAsync();
    Task<CategoryResponseDto?> GetByIdAsync(int id);
    Task<CategoryResponseDto> AddAsync(CreateCategoryDto categoryDTO);
    Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto categoryDto);
    Task<CategoryResponseDto?> DeleteAsync(int id);
    Task<List<TodoResponseDto>?> GetTodosAsync(int id);
}