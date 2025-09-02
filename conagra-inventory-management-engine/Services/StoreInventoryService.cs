using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public class StoreInventoryService : IStoreInventoryService
{
    private readonly IStoreInventoryRepository _storeInventoryRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public StoreInventoryService(IStoreInventoryRepository storeInventoryRepository, IWarehouseRepository warehouseRepository)
    {
        _storeInventoryRepository = storeInventoryRepository;
        _warehouseRepository = warehouseRepository;
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

        // Apply store name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.StoreName))
        {
            storeInventory = storeInventory.Where(si => 
                si.Store?.Name.Contains(queryParameters.StoreName, StringComparison.OrdinalIgnoreCase) == true);
        }

        // Apply product name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.ProductName))
        {
            storeInventory = storeInventory.Where(si => 
                si.Product?.Name.Contains(queryParameters.ProductName, StringComparison.OrdinalIgnoreCase) == true);
        }

        if (queryParameters.BelowThreshold.HasValue && queryParameters.BelowThreshold.Value)
        {
            // This would need to be enhanced to actually check against thresholds
            // For now, we'll use a simple quantity check
            storeInventory = storeInventory.Where(si => si.Quantity < 10); // Example threshold
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

    public async Task<StoreInventoryDto> CreateStoreInventoryAsync(CreateStoreInventoryDto createStoreInventoryDto)
    {
        // Validate that the store exists
        var storeExists = await _storeInventoryRepository.StoreExistsAsync(createStoreInventoryDto.StoreId);
        if (!storeExists)
        {
            throw new InvalidOperationException($"Store with ID {createStoreInventoryDto.StoreId} does not exist.");
        }

        // Validate that the product exists
        var productExists = await _storeInventoryRepository.ProductExistsAsync(createStoreInventoryDto.ProductId);
        if (!productExists)
        {
            throw new InvalidOperationException($"Product with ID {createStoreInventoryDto.ProductId} does not exist.");
        }

        // Check warehouse inventory for the product
        var warehouseInventory = await _warehouseRepository.GetWarehouseInventoryByProductIdAsync(createStoreInventoryDto.ProductId);
        if (warehouseInventory == null)
        {
            throw new InvalidOperationException($"Product with ID {createStoreInventoryDto.ProductId} is not available in warehouse inventory.");
        }

        // Check if warehouse has enough quantity
        if (warehouseInventory.Quantity < createStoreInventoryDto.Quantity)
        {
            throw new InvalidOperationException($"Insufficient warehouse inventory. Available: {warehouseInventory.Quantity}, Requested: {createStoreInventoryDto.Quantity}");
        }

        // Check if store inventory already exists for this store and product combination
        var existingInventory = await _storeInventoryRepository.GetStoreInventoryByStoreAndProductAsync(createStoreInventoryDto.StoreId, createStoreInventoryDto.ProductId);
        
        StoreInventoryDto result;
        
        if (existingInventory != null)
        {
            // Add to existing inventory
            var newQuantity = existingInventory.Quantity + createStoreInventoryDto.Quantity;
            
            // Update the existing store inventory
            var updatedInventory = await _storeInventoryRepository.UpdateStoreInventoryQuantityAsync(createStoreInventoryDto.StoreId, createStoreInventoryDto.ProductId, newQuantity);
            
            result = new StoreInventoryDto 
            { 
                Id = updatedInventory.Id, 
                StoreId = updatedInventory.StoreId, 
                ProductId = updatedInventory.ProductId, 
                Quantity = updatedInventory.Quantity,
                StoreName = null, // Will be populated if needed
                ProductName = null // Will be populated if needed
            };
        }
        else
        {
            // Create new store inventory record
            var lastId = await _storeInventoryRepository.GetLastStoreInventoryIdAsync();
            var newId = lastId + 1;

            var storeInventory = new StoreInventory
            {
                Id = newId,
                StoreId = createStoreInventoryDto.StoreId,
                ProductId = createStoreInventoryDto.ProductId,
                Quantity = createStoreInventoryDto.Quantity
            };

            var createdStoreInventory = await _storeInventoryRepository.CreateStoreInventoryAsync(storeInventory);
            
            result = new StoreInventoryDto 
            { 
                Id = createdStoreInventory.Id, 
                StoreId = createdStoreInventory.StoreId, 
                ProductId = createdStoreInventory.ProductId, 
                Quantity = createdStoreInventory.Quantity,
                StoreName = null, // Will be populated if needed
                ProductName = null // Will be populated if needed
            };
        }

        // Update warehouse inventory by subtracting the shipped quantity
        var newWarehouseQuantity = warehouseInventory.Quantity - createStoreInventoryDto.Quantity;
        var warehouseUpdated = await _warehouseRepository.UpdateWarehouseQuantityAsync(createStoreInventoryDto.ProductId, newWarehouseQuantity);
        
        if (!warehouseUpdated)
        {
            // If warehouse update failed, we should ideally rollback the store inventory changes
            // For now, we'll throw an exception to indicate the issue
            throw new InvalidOperationException("Store inventory updated but failed to update warehouse inventory. Please contact administrator.");
        }

        return result;
    }
}
