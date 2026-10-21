using System.ComponentModel.DataAnnotations;
namespace TodoApi.Dto;

public class CreateTodoDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title = string.Empty;
    public string Description = string.Empty;
}