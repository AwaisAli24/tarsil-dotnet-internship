using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

public class UpdateProductDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue)]
    public double Price { get; set; }
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }

}