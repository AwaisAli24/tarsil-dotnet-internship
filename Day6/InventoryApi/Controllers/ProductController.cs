using InventoryApi.DTOs;
using InventoryApi.Services;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ResponseProductDTO>>> Get([FromQuery] int? categoryId, [FromQuery] string? search, [FromQuery] double? minPrice, [FromQuery] double? maxPrice, [FromQuery] int? pageSize, [FromQuery] int? page)
    {
        var products = await _productService.GetProductsAsync(categoryId, search, minPrice, maxPrice, pageSize, page);
        return Ok(products);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResponseProductDTO?>> GetById(int id)
    {
        var product = await _productService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<ResponseProductDTO>> Add(CreateProductDTO product)
    {
        ResponseProductDTO toAdd;
        try
        {

            toAdd = await _productService.AddProductAsync(product);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = toAdd.Id },
            value: toAdd
        );
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ResponseProductDTO?>> Put(int id, UpdateProductDTO product)
    {
        ResponseProductDTO? toUpdate;
        try
        {
            toUpdate = await _productService.UpdateProductAsync(id, product);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        if (toUpdate == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ResponseProductDTO?>> Delete(int id)
    {
        var toDelete = await _productService.DeleteProductAsync(id);
        if (toDelete == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpGet("low-stock")]
    public async Task<ActionResult<List<ResponseProductDTO?>>> GetThreshold([FromQuery] int threshold)
    {
        var products = await _productService.GetProductsByThreshold(threshold);
        return Ok(products);
    }
    [HttpPost("{id:int}/stock")]
    public async Task<ActionResult<ResponseProductDTO?>> UpdateStock(int id, UpdateStockDTO stock)
    {
        ResponseProductDTO? product;
        try
        {
            product = await _productService.UpdateStock(id, stock.change);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        if (product == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}
