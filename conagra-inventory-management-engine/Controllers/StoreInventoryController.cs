using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

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
    public async Task<ActionResult<PagedResult<StoreInventoryDto>>> GetStoreInventory([FromQuery] StoreInventoryQueryParameters queryParameters)
    {
        try
        {
            var result = await _storeInventoryService.GetStoreInventoryAsync(queryParameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("store/{storeId}/product/{productId}")]
    public async Task<ActionResult<StoreInventoryDto>> GetStoreInventoryByStoreAndProduct(int storeId, int productId)
    {
        try
        {
            var storeInventory = await _storeInventoryService.GetStoreInventoryByStoreAndProductAsync(storeId, productId);
            if (storeInventory == null)
                return NotFound($"No inventory found for store ID {storeId} and product ID {productId}");

            return Ok(storeInventory);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
