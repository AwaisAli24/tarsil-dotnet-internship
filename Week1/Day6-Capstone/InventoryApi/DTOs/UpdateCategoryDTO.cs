using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

public class UpdateCategoryDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}