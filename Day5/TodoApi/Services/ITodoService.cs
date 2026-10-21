using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Dto;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<List<Todo>> GetAsync();
    Task<Todo?> GetByIDAsync(int id);
    Task<Todo> AddAsync(CreateTodoDto dto);
    Task<Todo?> UpdateByIDAsync(int id, UpdateTodoDto dto);
    Task<Todo?> DeleteByIDAsync(int id);
    Task<List<Todo>> GetCompletedAsync();
    Task<Todo?> CompleteAsync(int id);
}