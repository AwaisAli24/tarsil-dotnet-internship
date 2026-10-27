using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dto;

public class UpdateCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}