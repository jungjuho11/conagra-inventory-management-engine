using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Services;

public interface IStoreInventoryService
{
    Task<IEnumerable<StoreInventory>> GetAllStoreInventoryAsync();
    Task<IEnumerable<StoreInventory>> GetStoreInventoryByStoreAsync(int storeId);
    Task<StoreInventory?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId);
    Task<IEnumerable<StoreInventory>> GetStoresBelowThresholdAsync();
}
