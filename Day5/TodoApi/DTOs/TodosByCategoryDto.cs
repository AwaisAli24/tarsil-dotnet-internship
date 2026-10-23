namespace TodoApi.Dto;

public class TodosByCategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<TodoResponseDto> Todos { get; set; } = new List<TodoResponseDto>();
}