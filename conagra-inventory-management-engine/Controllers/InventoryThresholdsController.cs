using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryThresholdsController : ControllerBase
{
    private readonly IInventoryThresholdsService _inventoryThresholdsService;

    public InventoryThresholdsController(IInventoryThresholdsService inventoryThresholdsService)
    {
        _inventoryThresholdsService = inventoryThresholdsService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<InventoryThresholdDto>>> GetInventoryThresholds([FromQuery] InventoryThresholdQueryParameters queryParameters)
    {
        try
        {
            var result = await _inventoryThresholdsService.GetInventoryThresholdsAsync(queryParameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("store/{storeId}/product/{productId}")]
    public async Task<ActionResult<InventoryThresholdDto>> GetInventoryThresholdByStoreAndProduct(int storeId, int productId)
    {
        try
        {
            var inventoryThreshold = await _inventoryThresholdsService.GetInventoryThresholdByStoreAndProductAsync(storeId, productId);
            if (inventoryThreshold == null)
                return NotFound($"No inventory threshold found for store ID {storeId} and product ID {productId}");

            return Ok(inventoryThreshold);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
