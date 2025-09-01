using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface IInventoryThresholdsService
{
    Task<PagedResult<InventoryThresholdDto>> GetInventoryThresholdsAsync(InventoryThresholdQueryParameters queryParameters);
    Task<InventoryThresholdDto?> GetInventoryThresholdByStoreAndProductAsync(int storeId, int productId);
}
