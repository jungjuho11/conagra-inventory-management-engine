using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Repositories;

public interface IInventoryThresholdRepository
{
    Task<IEnumerable<InventoryThreshold>> GetAllInventoryThresholdsAsync();
    Task<InventoryThreshold?> GetInventoryThresholdByStoreAndProductAsync(int storeId, int productId);
    Task<bool> StoreExistsAsync(int storeId);
    Task<bool> ProductExistsAsync(int productId);
    Task<int> GetLastInventoryThresholdIdAsync();
    Task<InventoryThreshold> CreateInventoryThresholdAsync(InventoryThreshold inventoryThreshold);
    Task<InventoryThreshold> UpdateInventoryThresholdQuantityAsync(int storeId, int productId, int newThresholdQuantity);
}
