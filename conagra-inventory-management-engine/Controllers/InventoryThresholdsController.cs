using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;

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
    public async Task<ActionResult<IEnumerable<object>>> GetAllInventoryThresholds()
    {
        try
        {
            var inventoryThresholds = await _inventoryThresholdsService.GetAllInventoryThresholdsAsync();
            var thresholdDtos = inventoryThresholds.Select(it => new { it.Id, it.StoreId, it.ProductId, it.ThresholdQuantity }).ToList();
            return Ok(thresholdDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("store/{storeId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetInventoryThresholdsByStore(int storeId)
    {
        try
        {
            var inventoryThresholds = await _inventoryThresholdsService.GetInventoryThresholdsByStoreAsync(storeId);
            var thresholdDtos = inventoryThresholds.Select(it => new { it.Id, it.StoreId, it.ProductId, it.ThresholdQuantity }).ToList();
            return Ok(thresholdDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetInventoryThresholdsByProduct(int productId)
    {
        try
        {
            var inventoryThresholds = await _inventoryThresholdsService.GetInventoryThresholdsByProductAsync(productId);
            var thresholdDtos = inventoryThresholds.Select(it => new { it.Id, it.StoreId, it.ProductId, it.ThresholdQuantity }).ToList();
            return Ok(thresholdDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("store/{storeId}/product/{productId}")]
    public async Task<ActionResult<object>> GetInventoryThresholdByStoreAndProduct(int storeId, int productId)
    {
        try
        {
            var inventoryThreshold = await _inventoryThresholdsService.GetInventoryThresholdByStoreAndProductAsync(storeId, productId);
            if (inventoryThreshold == null)
                return NotFound($"No inventory threshold found for store ID {storeId} and product ID {productId}");

            return Ok(new { inventoryThreshold.Id, inventoryThreshold.StoreId, inventoryThreshold.ProductId, inventoryThreshold.ThresholdQuantity });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
