using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;

namespace conagra_inventory_management_engine.Services;

public class StoreInventoryService : IStoreInventoryService
{
    private readonly IStoreInventoryRepository _storeInventoryRepository;

    public StoreInventoryService(IStoreInventoryRepository storeInventoryRepository)
    {
        _storeInventoryRepository = storeInventoryRepository;
    }

    public async Task<IEnumerable<StoreInventory>> GetAllStoreInventoryAsync()
    {
        return await _storeInventoryRepository.GetAllStoresInventoryAsync();
    }

    public async Task<IEnumerable<StoreInventory>> GetStoreInventoryByStoreAsync(int storeId)
    {
        return await _storeInventoryRepository.GetStoreInventoryByStoreIdAsync(storeId);
    }

    public async Task<StoreInventory?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId)
    {
        return await _storeInventoryRepository.GetStoreInventoryByStoreAndProductAsync(storeId, productId);
    }

    public async Task<IEnumerable<StoreInventory>> GetStoresBelowThresholdAsync()
    {
        return await _storeInventoryRepository.GetStoresBelowThresholdAsync();
    }
}
