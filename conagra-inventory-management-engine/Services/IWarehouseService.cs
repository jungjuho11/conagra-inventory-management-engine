using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface IWarehouseService
{
    Task<PagedResult<WarehouseDto>> GetWarehouseInventoryAsync(WarehouseQueryParameters queryParameters);
    Task<WarehouseDto?> GetWarehouseInventoryByProductAsync(int productId);
    Task<WarehouseDto?> GetWarehouseInventoryByProductNameAsync(string productName);
    Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto createWarehouseDto);
}
