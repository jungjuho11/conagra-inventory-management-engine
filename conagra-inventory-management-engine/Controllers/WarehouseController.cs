using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<WarehouseDto>>> GetWarehouseInventory([FromQuery] WarehouseQueryParameters queryParameters)
    {
        try
        {
            var result = await _warehouseService.GetWarehouseInventoryAsync(queryParameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<WarehouseDto>> GetWarehouseInventoryByProduct(int productId)
    {
        try
        {
            var warehouseInventory = await _warehouseService.GetWarehouseInventoryByProductAsync(productId);
            if (warehouseInventory == null)
                return NotFound($"No warehouse inventory found for product ID {productId}");

            return Ok(warehouseInventory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("product/name/{productName}")]
    public async Task<ActionResult<WarehouseDto>> GetWarehouseInventoryByProductName(string productName)
    {
        try
        {
            var warehouseInventory = await _warehouseService.GetWarehouseInventoryByProductNameAsync(productName);
            if (warehouseInventory == null)
                return NotFound($"No warehouse inventory found for product name '{productName}'");

            return Ok(warehouseInventory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
