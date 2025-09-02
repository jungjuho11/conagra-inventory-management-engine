using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public class StoresService : IStoresService
{
    private readonly IStoreRepository _storeRepository;

    public StoresService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<PagedResult<StoreDto>> GetStoresAsync(StoreQueryParameters queryParameters)
    {
        var stores = await _storeRepository.GetAllStoresAsync();
        
        // Apply store ID filter
        if (queryParameters.StoreId.HasValue)
        {
            stores = stores.Where(s => s.Id == queryParameters.StoreId.Value);
        }
        
        // Apply store name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.StoreName))
        {
            stores = stores.Where(s => s.Name.Contains(queryParameters.StoreName, StringComparison.OrdinalIgnoreCase));
        }
        
        // Apply store address filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.StoreAddress))
        {
            stores = stores.Where(s => s.Address != null && s.Address.Contains(queryParameters.StoreAddress, StringComparison.OrdinalIgnoreCase));
        }
        
        // Apply search filter
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            stores = stores.Where(s => 
                s.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase) ||
                (s.Address != null && s.Address.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase)));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            stores = queryParameters.SortBy.ToLower() switch
            {
                "name" => queryParameters.SortOrder.ToLower() == "desc" 
                    ? stores.OrderByDescending(s => s.Name)
                    : stores.OrderBy(s => s.Name),
                "address" => queryParameters.SortOrder.ToLower() == "desc"
                    ? stores.OrderByDescending(s => s.Address)
                    : stores.OrderBy(s => s.Address),
                "id" => queryParameters.SortOrder.ToLower() == "desc"
                    ? stores.OrderByDescending(s => s.Id)
                    : stores.OrderBy(s => s.Id),
                _ => stores.OrderBy(s => s.Id)
            };
        }
        else
        {
            stores = stores.OrderBy(s => s.Id);
        }

        var totalCount = stores.Count();
        var pagedStores = stores
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(s => new StoreDto { Id = s.Id, Name = s.Name, Address = s.Address });

        return new PagedResult<StoreDto>
        {
            Data = pagedStores,
            Pagination = new PaginationInfo
            {
                Page = queryParameters.Page,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount
            }
        };
    }

    public async Task<StoreDto?> GetStoreByIdAsync(int storeId)
    {
        var store = await _storeRepository.GetStoreByIdAsync(storeId);
        return store == null ? null : new StoreDto { Id = store.Id, Name = store.Name, Address = store.Address };
    }
}
