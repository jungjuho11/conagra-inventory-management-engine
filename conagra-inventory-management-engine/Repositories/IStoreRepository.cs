using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Repositories;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetAllStoresAsync();
    Task<Store?> GetStoreByIdAsync(int id);
}
