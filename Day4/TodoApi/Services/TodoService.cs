using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    public List<Todo> todos;

    public TodoService()
    {
        todos = new List<Todo>();
    }
    public List<Todo> GetTodos()
    {
        return todos;
    }
    public Todo? GetTodoByID(int id)
    {
        return todos.FirstOrDefault(t => t.Id == id);
    }
    public Todo PostTodo(Todo todo)
    {
        try
        {
            todo.Id = todos.Max(t => t.Id) + 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            todo.Id = 1;
        }
        todo.IsCompleted = false;
        todo.CreatedAt = DateTime.Now;
        todos.Add(todo);
        return todo;
    }
    public Todo? UpdateTodoByID(int id, Todo todo)
    {
        Todo? toUpdate = todos.FirstOrDefault(t => t.Id == id);
        if (toUpdate == null)
        {
            return toUpdate;
        }
        toUpdate.Title = todo.Title;
        toUpdate.Description = todo.Description;
        return toUpdate;
    }
    public Todo? DeleteTodoByID(int id)
    {
        Todo? toDelete = todos.FirstOrDefault(t => t.Id == id);
        if (toDelete == null)
        {
            return toDelete;
        }
        todos.Remove(toDelete);
        return toDelete;
    }
    public Todo? Complete(int id)
    {
        Todo? toComplete = todos.FirstOrDefault(t => t.Id == id);
        if (toComplete == null)
        {
            return toComplete;
        }
        toComplete.IsCompleted = true;
        return toComplete;
    }
    public List<Todo>? GetCompleted()
    {
        List<Todo> completed = todos.Where(t => t.IsCompleted == true).ToList();
        return completed;
    }
}