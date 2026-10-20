using TodoApi.Models;
using TodoApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    // public List<Todo> todos;
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        // todos = new List<Todo>();
        _context = context;

    }
    public async Task<List<Todo>> GetTodos()
    {
        // return todos;
        var todos = await _context.Todos.ToListAsync();
        return todos;
    }
    public async Task<Todo?> GetTodoByID(int id)
    {
        return await _context.Todos.FindAsync(id);
    }
    public async Task<Todo> PostTodo(Todo todo)
    {
        // try
        // {
        //     todo.Id = todos.Max(t => t.Id) + 1;
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e.Message);
        //     todo.Id = 1;
        // }
        todo.IsCompleted = false;
        todo.CreatedAt = DateTime.Now;
        // todos.Add(todo);
        // return todo;
        await _context.AddAsync(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
    public async Task<Todo?> UpdateTodoByID(int id, Todo todo)
    {
        // Todo? toUpdate = todos.FirstOrDefault(t => t.Id == id);
        // if (toUpdate == null)
        // {
        //     return toUpdate;
        // }
        // toUpdate.Title = todo.Title;
        // toUpdate.Description = todo.Description;
        // return toUpdate;
        Todo? toUpdate = await _context.Todos.FindAsync(id);
        if (toUpdate == null)
        {
            return null;
        }
        toUpdate.Title = todo.Title;
        toUpdate.Description = todo.Description;
        toUpdate.IsCompleted = todo.IsCompleted;

        await _context.SaveChangesAsync();

        return toUpdate;

    }
    public async Task<Todo?> DeleteTodoByID(int id)
    {
        // Todo? toDelete = todos.FirstOrDefault(t => t.Id == id);
        // if (toDelete == null)
        // {
        //     return toDelete;
        // }
        // todos.Remove(toDelete);
        // return toDelete;
        Todo? toDelete = await _context.Todos.FindAsync(id);

        if (toDelete is null)
        {
            return null;
        }

        _context.Todos.Remove(toDelete);
        await _context.SaveChangesAsync();

        return toDelete;
    }
    public async Task<Todo?> Complete(int id)
    {
        // Todo? toComplete = todos.FirstOrDefault(t => t.Id == id);
        // if (toComplete == null)
        // {
        //     return toComplete;
        // }
        // toComplete.IsCompleted = true;
        // return toComplete;
        Todo? toComplete = await _context.Todos.FindAsync(id);

        if (toComplete is null)
        {
            return null;
        }

        toComplete.IsCompleted = true;

        await _context.SaveChangesAsync();

        return toComplete;
    }
    public async Task<List<Todo>> GetCompleted()
    {
        // List<Todo> completed = todos.Where(t => t.IsCompleted == true).ToList();
        // return completed;
        return await _context.Todos
            .Where(t => t.IsCompleted)
            .ToListAsync();
    }
}