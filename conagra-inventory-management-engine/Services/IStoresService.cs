using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Services;

public interface IStoresService
{
    Task<IEnumerable<Store>> GetAllStoresAsync();
    Task<Store?> GetStoreByIdAsync(int storeId);
}
