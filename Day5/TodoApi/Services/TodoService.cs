using TodoApi.Models;
using TodoApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Todo>> GetTodos()
    {
        var todos = await _context.Todos.ToListAsync();
        return todos;
    }
    public async Task<Todo?> GetTodoByID(int id)
    {
        return await _context.Todos.FindAsync(id);
    }
    public async Task<Todo> PostTodo(Todo todo)
    {
        todo.IsCompleted = false;
        todo.CreatedAt = DateTime.Now;
        await _context.AddAsync(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
    public async Task<Todo?> UpdateTodoByID(int id, Todo todo)
    {
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
        return await _context.Todos
            .Where(t => t.IsCompleted)
            .ToListAsync();
    }
}