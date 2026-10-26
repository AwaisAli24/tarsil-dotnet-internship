using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

class ProductService : IProductService
{
    private readonly AppDbContext _context;
    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<ResponseProductDTO>> GetProductsAsync(int? categoryId, string? search, double? minPrice, double? maxPrice, int? pageSize, int? page)
    {
        var query = _context.Products.AsQueryable();
        if (categoryId != null)
        {
            query = query.Where(p => p.CategoryId == categoryId);
            Console.WriteLine("Here 1");
        }
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            Console.WriteLine("Here 2");
        }
        if (minPrice != null)
        {
            query = query.Where(p => p.Price > minPrice);
            Console.WriteLine("Here 3");
        }
        if (maxPrice != null)
        {
            query = query.Where(p => p.Price <= maxPrice);
            Console.WriteLine("Here 4");
        }
        var products = await query.ToListAsync();
        if (pageSize != null && page != null)
        {
            int skip = (int)((page - 1) * pageSize);
            products = products.Skip(skip).Take((int)pageSize).ToList();
        }
        var productsDtoList = products.Select(MapToDto).ToList();
        return productsDtoList;
    }
    public async Task<ResponseProductDTO?> GetProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return null;
        }
        return MapToDto(product);
    }
    public async Task<ResponseProductDTO> AddProductAsync(CreateProductDTO product)
    {
        bool isDuplicate = await _context.Products.AnyAsync(p => p.Sku == product.Sku);
        if (isDuplicate)
        {
            throw new InvalidOperationException("A product with this Sku already exists");
        }
        Product toAdd = new Product
        {
            Name = product.Name,
            Sku = product.Sku,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = DateTime.Now,
            CategoryId = product.CategoryId
        };
        await _context.Products.AddAsync(toAdd);
        await _context.SaveChangesAsync();
        return MapToDto(toAdd);
    }
    public async Task<ResponseProductDTO?> UpdateProductAsync(int id, UpdateProductDTO product)
    {
        bool isDuplicate = await _context.Products.Where(p => p.Id != id).AnyAsync(p => p.Sku == product.Sku);
        if (isDuplicate)
        {
            throw new InvalidOperationException("A product with this Sku already exists");
        }
        var toUpdate = await _context.Products.FindAsync(id);
        if (toUpdate == null)
        {
            return null;
        }
        toUpdate.Name = product.Name;
        toUpdate.Sku = product.Sku;
        toUpdate.Price = product.Price;
        toUpdate.StockQuantity = product.StockQuantity;
        toUpdate.CategoryId = product.CategoryId;
        await _context.SaveChangesAsync();
        return MapToDto(toUpdate);
    }
    public async Task<ResponseProductDTO?> DeleteProductAsync(int id)
    {
        var toDelete = await _context.Products.FindAsync(id);
        if (toDelete == null)
        {
            return null;
        }
        _context.Products.Remove(toDelete);
        await _context.SaveChangesAsync();
        return MapToDto(toDelete);
    }
    public async Task<List<ResponseProductDTO>> GetProductsByThreshold(int val)
    {
        var products = await _context.Products.Where(p => p.StockQuantity <= val).ToListAsync();
        return products.Select(MapToDto).ToList();
    }
    public async Task<ResponseProductDTO?> UpdateStock(int id, int change)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return null;
        }
        product.StockQuantity += change;
        await _context.SaveChangesAsync();
        return MapToDto(product);
    }
    public static ResponseProductDTO MapToDto(Product product)
    {
        ResponseProductDTO productDto = new ResponseProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Sku = product.Sku,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt,
            CategoryId = product.CategoryId
        };
        return productDto;
    }
}