namespace TodoApi.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Todo> Todos { get; set; } = new List<Todo>();
}