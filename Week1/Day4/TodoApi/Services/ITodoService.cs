using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    List<Todo> GetTodos();
    Todo? GetTodoByID(int id);
    Todo PostTodo(Todo todo);
    Todo? UpdateTodoByID(int id, Todo todo);
    Todo? DeleteTodoByID(int id);
    List<Todo>? GetCompleted();
    Todo? Complete(int id);
}