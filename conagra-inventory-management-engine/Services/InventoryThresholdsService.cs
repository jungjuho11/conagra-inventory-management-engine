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
        var thresholds = inventoryThresholds.Models.ToList();
        
        // Load store and product information for each threshold
        foreach (var threshold in thresholds)
        {
            // Load store information
            if (threshold.StoreId > 0)
            {
                var storeResponse = await _supabaseClient
                    .From<Store>()
                    .Where(x => x.Id == threshold.StoreId)
                    .Get();
                
                threshold.Store = storeResponse.Models.FirstOrDefault();
            }
            
            // Load product information
            if (threshold.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == threshold.ProductId)
                    .Get();
                
                threshold.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        var thresholdsEnumerable = thresholds.AsEnumerable();
        
        // Apply filters
        if (queryParameters.StoreId.HasValue)
        {
            thresholdsEnumerable = thresholdsEnumerable.Where(it => it.StoreId == queryParameters.StoreId.Value);
        }

        if (queryParameters.ProductId.HasValue)
        {
            thresholdsEnumerable = thresholdsEnumerable.Where(it => it.ProductId == queryParameters.ProductId.Value);
        }

        // Apply store name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.StoreName))
        {
            thresholdsEnumerable = thresholdsEnumerable.Where(it => 
                it.Store?.Name.Contains(queryParameters.StoreName, StringComparison.OrdinalIgnoreCase) == true);
        }

        // Apply product name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.ProductName))
        {
            thresholdsEnumerable = thresholdsEnumerable.Where(it => 
                it.Product?.Name.Contains(queryParameters.ProductName, StringComparison.OrdinalIgnoreCase) == true);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            thresholdsEnumerable = queryParameters.SortBy.ToLower() switch
            {
                "storename" => queryParameters.SortOrder.ToLower() == "desc" 
                    ? thresholdsEnumerable.OrderByDescending(it => it.Store?.Name)
                    : thresholdsEnumerable.OrderBy(it => it.Store?.Name),
                "productname" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholdsEnumerable.OrderByDescending(it => it.Product?.Name)
                    : thresholdsEnumerable.OrderBy(it => it.Product?.Name),
                "thresholdquantity" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholdsEnumerable.OrderByDescending(it => it.ThresholdQuantity)
                    : thresholdsEnumerable.OrderBy(it => it.ThresholdQuantity),
                "storeid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholdsEnumerable.OrderByDescending(it => it.StoreId)
                    : thresholdsEnumerable.OrderBy(it => it.StoreId),
                "productid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? thresholdsEnumerable.OrderByDescending(it => it.ProductId)
                    : thresholdsEnumerable.OrderBy(it => it.ProductId),
                _ => thresholdsEnumerable.OrderBy(it => it.Id)
            };
        }
        else
        {
            thresholdsEnumerable = thresholdsEnumerable.OrderBy(it => it.Id);
        }

        var totalCount = thresholdsEnumerable.Count();
        var pagedThresholds = thresholdsEnumerable
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
        
        if (threshold == null)
            return null;
        
        // Load store information
        if (threshold.StoreId > 0)
        {
            var storeResponse = await _supabaseClient
                .From<Store>()
                .Where(x => x.Id == threshold.StoreId)
                .Get();
            
            threshold.Store = storeResponse.Models.FirstOrDefault();
        }
        
        // Load product information
        if (threshold.ProductId > 0)
        {
            var productResponse = await _supabaseClient
                .From<Product>()
                .Where(x => x.Id == threshold.ProductId)
                .Get();
            
            threshold.Product = productResponse.Models.FirstOrDefault();
        }
        
        return new InventoryThresholdDto 
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
