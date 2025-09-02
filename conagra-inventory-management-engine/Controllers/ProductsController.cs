using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

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
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts([FromQuery] ProductQueryParameters queryParameters)
    {
        try
        {
            var result = await _productsService.GetProductsAsync(queryParameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int productId)
    {
        try
        {
            var product = await _productsService.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound($"Product with ID {productId} not found");

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name))
            {
                return BadRequest("Product name is required");
            }

            var createdProduct = await _productsService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProductById), new { productId = createdProduct.Id }, createdProduct);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
