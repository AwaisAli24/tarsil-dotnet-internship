using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<List<Todo>> GetTodos();
    Task<Todo?> GetTodoByID(int id);
    Task<Todo> PostTodo(Todo todo);
    Task<Todo?> UpdateTodoByID(int id, Todo todo);
    Task<Todo?> DeleteTodoByID(int id);
    Task<List<Todo>> GetCompleted();
    Task<Todo?> Complete(int id);
}