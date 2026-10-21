using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<List<Todo>> GetAsync();
    Task<Todo?> GetByIDAsync(int id);
    Task<Todo> AddAsync(Todo todo);
    Task<Todo?> UpdateByIDAsync(int id, Todo todo);
    Task<Todo?> DeleteByIDAsync(int id);
    Task<List<Todo>> GetCompletedAsync();
    Task<Todo?> CompleteAsync(int id);
}