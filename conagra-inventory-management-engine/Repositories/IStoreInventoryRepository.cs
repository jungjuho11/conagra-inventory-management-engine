using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Repositories;

public interface IStoreInventoryRepository
{
    Task<IEnumerable<StoreInventory>> GetAllStoresInventoryAsync();
    Task<IEnumerable<StoreInventory>> GetStoreInventoryByStoreIdAsync(int storeId);
    Task<StoreInventory?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId);
    Task<StoreInventory> ShipProductToStoreAsync(int storeId, int productId, int quantity);
    Task<IEnumerable<StoreInventory>> GetStoresBelowThresholdAsync();
    Task<bool> StoreExistsAsync(int storeId);
    Task<bool> ProductExistsAsync(int productId);
    Task<int> GetLastStoreInventoryIdAsync();
    Task<StoreInventory> CreateStoreInventoryAsync(StoreInventory storeInventory);
    Task<StoreInventory> UpdateStoreInventoryQuantityAsync(int storeId, int productId, int newQuantity);
}
