using TodoApi.Models;
using TodoApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dto;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<TodoResponseDto>> GetAsync(bool? isCompleted, string? search)
    {
        var query = _context.Todos.AsQueryable();
        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(t => t.Title.Contains(search));
        }
        var todos = await query.ToListAsync();
        var result = todos.Select(MapToDTO).ToList();
        return result;
    }
    public async Task<TodoResponseDto?> GetByIDAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return null;
        }
        TodoResponseDto result = MapToDTO(todo);
        Console.WriteLine(result.Title);
        return result;
    }
    public async Task<TodoResponseDto> AddAsync(CreateTodoDto dto)
    {
        bool exists = await IsCategoryExists(dto.CategoryId);
        if (!exists)
        {
            throw new BadHttpRequestException("Category not found", StatusCodes.Status400BadRequest);
        }
        Todo todo = new Todo
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.Now,
            CategoryId = dto.CategoryId
        };
        await _context.AddAsync(todo);
        await _context.SaveChangesAsync();
        return MapToDTO(todo);
    }
    public async Task<TodoResponseDto?> UpdateByIDAsync(int id, UpdateTodoDto dto)
    {
        bool exists = await IsCategoryExists(dto.CategoryId);
        if (!exists)
        {
            throw new BadHttpRequestException("Category not found", StatusCodes.Status400BadRequest);
        }
        Todo? toUpdate = await _context.Todos.FindAsync(id);
        if (toUpdate == null)
        {
            return null;
        }
        toUpdate.Title = dto.Title;
        toUpdate.Description = dto.Description;
        toUpdate.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return MapToDTO(toUpdate);

    }
    public async Task<TodoResponseDto?> DeleteByIDAsync(int id)
    {
        Todo? toDelete = await _context.Todos.FindAsync(id);
        if (toDelete is null)
        {
            return null;
        }
        _context.Todos.Remove(toDelete);
        await _context.SaveChangesAsync();
        return MapToDTO(toDelete);
    }
    public async Task<TodoResponseDto?> CompleteAsync(int id)
    {
        Todo? toComplete = await _context.Todos.FindAsync(id);
        if (toComplete is null)
        {
            return null;
        }
        toComplete.IsCompleted = true;
        await _context.SaveChangesAsync();
        return MapToDTO(toComplete);
    }
    public async Task<List<TodoResponseDto>> GetCompletedAsync()
    {
        var completedTasks = await _context.Todos.Where(t => t.IsCompleted).ToListAsync();
        var completedTasksDTO = completedTasks.Select(MapToDTO).ToList();
        return completedTasksDTO;
    }
    public static TodoResponseDto MapToDTO(Todo todo)
    {
        return new TodoResponseDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            CreatedAt = todo.CreatedAt,
            CategoryId = todo.CategoryId
        };
    }
    public async Task<bool> IsCategoryExists(int id)
    {
        bool exists = await _context.Categories.AnyAsync(t => t.Id == id);
        return exists;
    }
}
