using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Services;

public interface IInventoryThresholdsService
{
    Task<IEnumerable<InventoryThreshold>> GetAllInventoryThresholdsAsync();
    Task<IEnumerable<InventoryThreshold>> GetInventoryThresholdsByStoreAsync(int storeId);
    Task<IEnumerable<InventoryThreshold>> GetInventoryThresholdsByProductAsync(int productId);
    Task<InventoryThreshold?> GetInventoryThresholdByStoreAndProductAsync(int storeId, int productId);
}
