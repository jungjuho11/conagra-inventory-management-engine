using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;

namespace conagra_inventory_management_engine.Services;

public class StoresService : IStoresService
{
    private readonly IStoreRepository _storeRepository;

    public StoresService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<IEnumerable<Store>> GetAllStoresAsync()
    {
        return await _storeRepository.GetAllStoresAsync();
    }

    public async Task<Store?> GetStoreByIdAsync(int storeId)
    {
        return await _storeRepository.GetStoreByIdAsync(storeId);
    }
}
