using Microsoft.AspNetCore.Mvc;
using conagra_inventory_management_engine.Services;

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
    public async Task<ActionResult<IEnumerable<object>>> GetAllStores()
    {
        try
        {
            var stores = await _storesService.GetAllStoresAsync();
            var storeDtos = stores.Select(s => new { s.Id, s.Name, s.Address }).ToList();
            return Ok(storeDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{storeId}")]
    public async Task<ActionResult<object>> GetStoreById(int storeId)
    {
        try
        {
            var store = await _storesService.GetStoreByIdAsync(storeId);
            if (store == null)
                return NotFound($"Store with ID {storeId} not found");

            return Ok(new { store.Id, store.Name, store.Address });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
