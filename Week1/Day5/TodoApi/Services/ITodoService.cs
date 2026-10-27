using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Dto;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<List<TodoResponseDto>> GetAsync(bool? isCompleted, string? search);
    Task<TodoResponseDto?> GetByIDAsync(int id);
    Task<TodoResponseDto> AddAsync(CreateTodoDto dto);
    Task<TodoResponseDto?> UpdateByIDAsync(int id, UpdateTodoDto dto);
    Task<TodoResponseDto?> DeleteByIDAsync(int id);
    Task<List<TodoResponseDto>> GetCompletedAsync();
    Task<TodoResponseDto?> CompleteAsync(int id);
}