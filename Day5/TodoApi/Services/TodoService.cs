using TodoApi.Models;
using TodoApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dto;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Todo>> GetAsync()
    {
        var todos = await _context.Todos.ToListAsync();
        return todos;
    }
    public async Task<Todo?> GetByIDAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }
    public async Task<Todo> AddAsync(CreateTodoDto dto)
    {
        Todo todo = new Todo
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };
        await _context.AddAsync(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
    public async Task<Todo?> UpdateByIDAsync(int id, UpdateTodoDto dto)
    {
        Todo? toUpdate = await _context.Todos.FindAsync(id);
        if (toUpdate == null)
        {
            return null;
        }
        toUpdate.Title = dto.Title;
        toUpdate.Description = dto.Description;

        await _context.SaveChangesAsync();

        return toUpdate;

    }
    public async Task<Todo?> DeleteByIDAsync(int id)
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
    public async Task<Todo?> CompleteAsync(int id)
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
    public async Task<List<Todo>> GetCompletedAsync()
    {
        return await _context.Todos
            .Where(t => t.IsCompleted)
            .ToListAsync();
    }
}