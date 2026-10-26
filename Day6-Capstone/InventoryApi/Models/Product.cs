namespace InventoryApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public double Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}