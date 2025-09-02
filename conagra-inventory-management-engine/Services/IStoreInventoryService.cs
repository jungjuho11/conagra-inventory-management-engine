using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface IStoreInventoryService
{
    Task<PagedResult<StoreInventoryDto>> GetStoreInventoryAsync(StoreInventoryQueryParameters queryParameters);
    Task<StoreInventoryDto?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId);
    Task<StoreInventoryDto> CreateStoreInventoryAsync(CreateStoreInventoryDto createStoreInventoryDto);
}
