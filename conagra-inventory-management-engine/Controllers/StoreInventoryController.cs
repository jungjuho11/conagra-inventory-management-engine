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

    [HttpPost]
    public async Task<ActionResult<StoreInventoryDto>> CreateStoreInventory([FromBody] CreateStoreInventoryDto createStoreInventoryDto)
    {
        try
        {
            if (createStoreInventoryDto.StoreId <= 0)
            {
                return BadRequest("Store ID must be greater than 0");
            }

            if (createStoreInventoryDto.ProductId <= 0)
            {
                return BadRequest("Product ID must be greater than 0");
            }

            if (createStoreInventoryDto.Quantity < 0)
            {
                return BadRequest("Quantity cannot be negative");
            }

            var createdStoreInventory = await _storeInventoryService.CreateStoreInventoryAsync(createStoreInventoryDto);
            return CreatedAtAction(nameof(GetStoreInventoryByStoreAndProduct), new { storeId = createdStoreInventory.StoreId, productId = createdStoreInventory.ProductId }, createdStoreInventory);
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
