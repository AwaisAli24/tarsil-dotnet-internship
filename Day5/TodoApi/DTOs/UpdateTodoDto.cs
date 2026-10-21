using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dto;

public class UpdateTodoDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}