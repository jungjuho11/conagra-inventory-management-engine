using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface IStoresService
{
    Task<PagedResult<StoreDto>> GetStoresAsync(QueryParameters queryParameters);
    Task<StoreDto?> GetStoreByIdAsync(int storeId);
}
