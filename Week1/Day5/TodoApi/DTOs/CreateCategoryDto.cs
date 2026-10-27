namespace TodoApi.Dto;

using System.ComponentModel.DataAnnotations;

public class CreateCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}