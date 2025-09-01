using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public class StoreInventoryService : IStoreInventoryService
{
    private readonly IStoreInventoryRepository _storeInventoryRepository;

    public StoreInventoryService(IStoreInventoryRepository storeInventoryRepository)
    {
        _storeInventoryRepository = storeInventoryRepository;
    }

    public async Task<PagedResult<StoreInventoryDto>> GetStoreInventoryAsync(StoreInventoryQueryParameters queryParameters)
    {
        var storeInventory = await _storeInventoryRepository.GetAllStoresInventoryAsync();
        
        // Apply filters
        if (queryParameters.StoreId.HasValue)
        {
            storeInventory = storeInventory.Where(si => si.StoreId == queryParameters.StoreId.Value);
        }

        if (queryParameters.ProductId.HasValue)
        {
            storeInventory = storeInventory.Where(si => si.ProductId == queryParameters.ProductId.Value);
        }

        if (queryParameters.BelowThreshold.HasValue && queryParameters.BelowThreshold.Value)
        {
            // This would need to be enhanced to actually check against thresholds
            // For now, we'll use a simple quantity check
            storeInventory = storeInventory.Where(si => si.Quantity < 10); // Example threshold
        }

        // Apply search filter (search by store name or product name if available)
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            storeInventory = storeInventory.Where(si => 
                si.Store?.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase) == true ||
                si.Product?.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase) == true);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            storeInventory = queryParameters.SortBy.ToLower() switch
            {
                "storename" => queryParameters.SortOrder.ToLower() == "desc" 
                    ? storeInventory.OrderByDescending(si => si.Store?.Name)
                    : storeInventory.OrderBy(si => si.Store?.Name),
                "productname" => queryParameters.SortOrder.ToLower() == "desc"
                    ? storeInventory.OrderByDescending(si => si.Product?.Name)
                    : storeInventory.OrderBy(si => si.Product?.Name),
                "quantity" => queryParameters.SortOrder.ToLower() == "desc"
                    ? storeInventory.OrderByDescending(si => si.Quantity)
                    : storeInventory.OrderBy(si => si.Quantity),
                "storeid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? storeInventory.OrderByDescending(si => si.StoreId)
                    : storeInventory.OrderBy(si => si.StoreId),
                "productid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? storeInventory.OrderByDescending(si => si.ProductId)
                    : storeInventory.OrderBy(si => si.ProductId),
                _ => storeInventory.OrderBy(si => si.Id)
            };
        }
        else
        {
            storeInventory = storeInventory.OrderBy(si => si.Id);
        }

        var totalCount = storeInventory.Count();
        var pagedInventory = storeInventory
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(si => new StoreInventoryDto 
            { 
                Id = si.Id, 
                StoreId = si.StoreId, 
                ProductId = si.ProductId, 
                Quantity = si.Quantity,
                StoreName = si.Store?.Name,
                ProductName = si.Product?.Name
            });

        return new PagedResult<StoreInventoryDto>
        {
            Data = pagedInventory,
            Pagination = new PaginationInfo
            {
                Page = queryParameters.Page,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount
            }
        };
    }

    public async Task<StoreInventoryDto?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId)
    {
        var storeInventory = await _storeInventoryRepository.GetStoreInventoryByStoreAndProductAsync(storeId, productId);
        return storeInventory == null ? null : new StoreInventoryDto 
        { 
            Id = storeInventory.Id, 
            StoreId = storeInventory.StoreId, 
            ProductId = storeInventory.ProductId, 
            Quantity = storeInventory.Quantity,
            StoreName = storeInventory.Store?.Name,
            ProductName = storeInventory.Product?.Name
        };
    }
}
