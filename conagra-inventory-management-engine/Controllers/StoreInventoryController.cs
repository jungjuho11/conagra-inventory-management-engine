using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;

namespace conagra_inventory_management_engine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreInventoryController : ControllerBase
{
    private readonly IStoreInventoryService _storeInventoryService;

    public StoreInventoryController(IStoreInventoryService storeInventoryService)
    {
        _storeInventoryService = storeInventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAllStoreInventory()
    {
        try
        {
            var storeInventory = await _storeInventoryService.GetAllStoreInventoryAsync();
            var storeInventoryDtos = storeInventory.Select(si => new { si.Id, si.StoreId, si.ProductId, si.Quantity }).ToList();
            return Ok(storeInventoryDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("store/{storeId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetStoreInventoryByStore(int storeId)
    {
        try
        {
            var storeInventory = await _storeInventoryService.GetStoreInventoryByStoreAsync(storeId);
            var storeInventoryDtos = storeInventory.Select(si => new { si.Id, si.StoreId, si.ProductId, si.Quantity }).ToList();
            return Ok(storeInventoryDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetStoreInventoryByProduct(int productId)
    {
        try
        {
            // Get all store inventory and filter by product ID
            var allStoreInventory = await _storeInventoryService.GetAllStoreInventoryAsync();
            var filteredInventory = allStoreInventory.Where(si => si.ProductId == productId);
            var storeInventoryDtos = filteredInventory.Select(si => new { si.Id, si.StoreId, si.ProductId, si.Quantity }).ToList();
            return Ok(storeInventoryDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("store/{storeId}/product/{productId}")]
    public async Task<ActionResult<object>> GetStoreInventoryByStoreAndProduct(int storeId, int productId)
    {
        try
        {
            var storeInventory = await _storeInventoryService.GetStoreInventoryByStoreAndProductAsync(storeId, productId);
            if (storeInventory == null)
                return NotFound($"No inventory found for store ID {storeId} and product ID {productId}");

            return Ok(new { storeInventory.Id, storeInventory.StoreId, storeInventory.ProductId, storeInventory.Quantity });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("below-threshold")]
    public async Task<ActionResult<IEnumerable<object>>> GetStoresBelowThreshold()
    {
        try
        {
            var storesBelowThreshold = await _storeInventoryService.GetStoresBelowThresholdAsync();
            var storesBelowThresholdDtos = storesBelowThreshold.Select(si => new { si.Id, si.StoreId, si.ProductId, si.Quantity }).ToList();
            return Ok(storesBelowThresholdDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
