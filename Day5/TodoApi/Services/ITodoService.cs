using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    public List<Todo> GetTodos();
    public Todo? GetTodoByID(int id);
    public Todo PostTodo(Todo todo);
    public Todo? UpdateTodoByID(int id, Todo todo);
    public Todo? DeleteTodoByID(int id);
    public List<Todo>? GetCompleted();
    public Todo? Complete(int id);
}