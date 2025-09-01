using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;
using Supabase;

namespace conagra_inventory_management_engine.Services;

public class InventoryThresholdsService : IInventoryThresholdsService
{
    private readonly Supabase.Client _supabaseClient;

    public InventoryThresholdsService(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<PagedResult<InventoryThresholdDto>> GetInventoryThresholdsAsync(InventoryThresholdQueryParameters queryParameters)
    {
        var inventoryThresholds = await _supabaseClient.From<InventoryThreshold>().Get();
        var thresholds = inventoryThresholds.Models.AsEnumerable();
        
        // Apply filters
        if (queryParameters.StoreId.HasValue)
        {
            thresholds = thresholds.Where(it => it.StoreId == queryParameters.StoreId.Value);
        }

        if (queryParameters.ProductId.HasValue)
        {
            thresholds = thresholds.Where(it => it.ProductId == queryParameters.ProductId.Value);
        }

        // Apply search filter (search by store name or product name if available)
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            thresholds = thresholds.Where(it => 
                it.Store?.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase) == true ||
                it.Product?.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase) == true);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            thresholds = queryParameters.SortBy.ToLower() switch
            {
                "storename" => queryParameters.SortOrder.ToLower() == "desc" 
                    ? thresholds.OrderByDescending(it => it.Store?.Name)
                    : thresholds.OrderBy(it => it.Store?.Name),
                "productname" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholds.OrderByDescending(it => it.Product?.Name)
                    : thresholds.OrderBy(it => it.Product?.Name),
                "thresholdquantity" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholds.OrderByDescending(it => it.ThresholdQuantity)
                    : thresholds.OrderBy(it => it.ThresholdQuantity),
                "storeid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholds.OrderByDescending(it => it.StoreId)
                    : thresholds.OrderBy(it => it.StoreId),
                "productid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholds.OrderByDescending(it => it.ProductId)
                    : thresholds.OrderBy(it => it.ProductId),
                _ => thresholds.OrderBy(it => it.Id)
            };
        }
        else
        {
            thresholds = thresholds.OrderBy(it => it.Id);
        }

        var totalCount = thresholds.Count();
        var pagedThresholds = thresholds
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(it => new InventoryThresholdDto 
            { 
                Id = it.Id, 
                StoreId = it.StoreId, 
                ProductId = it.ProductId, 
                ThresholdQuantity = it.ThresholdQuantity,
                StoreName = it.Store?.Name,
                ProductName = it.Product?.Name
            });

        return new PagedResult<InventoryThresholdDto>
        {
            Data = pagedThresholds,
            Pagination = new PaginationInfo
            {
                Page = queryParameters.Page,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount
            }
        };
    }

    public async Task<InventoryThresholdDto?> GetInventoryThresholdByStoreAndProductAsync(int storeId, int productId)
    {
        var response = await _supabaseClient.From<InventoryThreshold>().Where(x => x.StoreId == storeId && x.ProductId == productId).Get();
        var threshold = response.Models.FirstOrDefault();
        return threshold == null ? null : new InventoryThresholdDto 
        { 
            Id = threshold.Id, 
            StoreId = threshold.StoreId, 
            ProductId = threshold.ProductId, 
            ThresholdQuantity = threshold.ThresholdQuantity,
            StoreName = threshold.Store?.Name,
            ProductName = threshold.Product?.Name
        };
    }
}
