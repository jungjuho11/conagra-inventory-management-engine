using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly IStoresService _storesService;

    public StoresController(IStoresService storesService)
    {
        _storesService = storesService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StoreDto>>> GetStores([FromQuery] StoreQueryParameters queryParameters)
    {
        try
        {
            var result = await _storesService.GetStoresAsync(queryParameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{storeId}")]
    public async Task<ActionResult<StoreDto>> GetStoreById(int storeId)
    {
        try
        {
            var store = await _storesService.GetStoreByIdAsync(storeId);
            if (store == null)
                return NotFound($"Store with ID {storeId} not found");

            return Ok(store);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<StoreDto>> CreateStore([FromBody] CreateStoreDto createStoreDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createStoreDto.Name))
            {
                return BadRequest("Store name is required");
            }

            if (string.IsNullOrWhiteSpace(createStoreDto.Address))
            {
                return BadRequest("Store address is required");
            }

            var createdStore = await _storesService.CreateStoreAsync(createStoreDto);
            return CreatedAtAction(nameof(GetStoreById), new { storeId = createdStore.Id }, createdStore);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
