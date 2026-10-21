using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Dto;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<List<TodoResponseDto>> GetAsync();
    Task<TodoResponseDto?> GetByIDAsync(int id);
    Task<Todo> AddAsync(CreateTodoDto dto);
    Task<Todo?> UpdateByIDAsync(int id, UpdateTodoDto dto);
    Task<Todo?> DeleteByIDAsync(int id);
    Task<List<Todo>> GetCompletedAsync();
    Task<Todo?> CompleteAsync(int id);
}