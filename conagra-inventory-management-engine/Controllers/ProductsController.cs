using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;

namespace conagra_inventory_management_engine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAllProducts()
    {
        try
        {
            var products = await _productsService.GetAllProductsAsync();
            var productDtos = products.Select(p => new { p.Id, p.Name }).ToList();
            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<object>> GetProductById(int productId)
    {
        try
        {
            var product = await _productsService.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound($"Product with ID {productId} not found");

            return Ok(new { product.Id, product.Name });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
