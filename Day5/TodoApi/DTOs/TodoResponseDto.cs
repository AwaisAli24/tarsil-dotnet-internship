namespace TodoApi.Dto;

public class TodoResponseDto
{
    public string Title = string.Empty;
    public string Description = string.Empty;
    public DateTime CreatedAt;
    public bool IsCompleted;
}