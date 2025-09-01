using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<PagedResult<WarehouseDto>> GetWarehouseInventoryAsync(WarehouseQueryParameters queryParameters)
    {
        var warehouseInventory = await _warehouseRepository.GetAllWarehouseInventoryAsync();
        
        // Apply product ID filter
        if (queryParameters.ProductId.HasValue)
        {
            warehouseInventory = warehouseInventory.Where(w => w.ProductId == queryParameters.ProductId.Value);
        }
        
        // Apply product name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.ProductName))
        {
            warehouseInventory = warehouseInventory.Where(w => 
                w.Product?.Name.Contains(queryParameters.ProductName, StringComparison.OrdinalIgnoreCase) == true);
        }
        
        // Apply search filter
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            warehouseInventory = warehouseInventory.Where(w => 
                w.Product?.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase) == true);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            warehouseInventory = queryParameters.SortBy.ToLower() switch
            {
                "productname" => queryParameters.SortOrder.ToLower() == "desc" 
                    ? warehouseInventory.OrderByDescending(w => w.Product?.Name)
                    : warehouseInventory.OrderBy(w => w.Product?.Name),
                "quantity" => queryParameters.SortOrder.ToLower() == "desc"
                    ? warehouseInventory.OrderByDescending(w => w.Quantity)
                    : warehouseInventory.OrderBy(w => w.Quantity),
                "productid" => queryParameters.SortOrder.ToLower() == "desc"
                    ? warehouseInventory.OrderByDescending(w => w.ProductId)
                    : warehouseInventory.OrderBy(w => w.ProductId),
                _ => warehouseInventory.OrderBy(w => w.Id)
            };
        }
        else
        {
            warehouseInventory = warehouseInventory.OrderBy(w => w.Id);
        }

        var totalCount = warehouseInventory.Count();
        var pagedInventory = warehouseInventory
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(w => new WarehouseDto 
            { 
                Id = w.Id, 
                ProductId = w.ProductId, 
                Quantity = w.Quantity,
                ProductName = w.Product?.Name
            });

        return new PagedResult<WarehouseDto>
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

    public async Task<WarehouseDto?> GetWarehouseInventoryByProductAsync(int productId)
    {
        var warehouseInventory = await _warehouseRepository.GetWarehouseInventoryByProductIdAsync(productId);
        return warehouseInventory == null ? null : new WarehouseDto 
        { 
            Id = warehouseInventory.Id, 
            ProductId = warehouseInventory.ProductId, 
            Quantity = warehouseInventory.Quantity,
            ProductName = warehouseInventory.Product?.Name
        };
    }

    public async Task<WarehouseDto?> GetWarehouseInventoryByProductNameAsync(string productName)
    {
        var warehouseInventory = await _warehouseRepository.GetWarehouseInventoryByProductNameAsync(productName);
        return warehouseInventory == null ? null : new WarehouseDto 
        { 
            Id = warehouseInventory.Id, 
            ProductId = warehouseInventory.ProductId, 
            Quantity = warehouseInventory.Quantity,
            ProductName = warehouseInventory.Product?.Name
        };
    }
}
