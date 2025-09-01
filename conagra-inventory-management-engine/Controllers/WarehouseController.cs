using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;

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
    public async Task<ActionResult<IEnumerable<object>>> GetAllWarehouseInventory()
    {
        try
        {
            var warehouseInventory = await _warehouseService.GetWarehouseInventoryAsync();
            var warehouseDtos = warehouseInventory.Select(w => new { w.Id, w.ProductId, w.Quantity }).ToList();
            return Ok(warehouseDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<object>> GetWarehouseInventoryByProduct(int productId)
    {
        try
        {
            var warehouseInventory = await _warehouseService.GetWarehouseInventoryByProductAsync(productId);
            if (warehouseInventory == null)
                return NotFound($"No warehouse inventory found for product ID {productId}");

            return Ok(new { warehouseInventory.Id, warehouseInventory.ProductId, warehouseInventory.Quantity });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
