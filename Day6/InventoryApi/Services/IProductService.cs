using InventoryApi.Models;
using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IProductService
{
    Task<List<ResponseProductDTO>> GetProductsAsync(int? categoryId, string? search, double? minPrice, double? maxPrice, int? pageSize, int? page);
    Task<ResponseProductDTO?> GetProductAsync(int id);
    Task<ResponseProductDTO> AddProductAsync(CreateProductDTO product);
    Task<ResponseProductDTO?> UpdateProductAsync(int id, UpdateProductDTO product);
    Task<ResponseProductDTO?> DeleteProductAsync(int id);
    Task<List<ResponseProductDTO>> GetProductsByThreshold(int val);
    Task<ResponseProductDTO?> UpdateStock(int id, int change);
}