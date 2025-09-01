using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Services;

public interface IWarehouseService
{
    Task<IEnumerable<Warehouse>> GetWarehouseInventoryAsync();
    Task<Warehouse?> GetWarehouseInventoryByProductAsync(int productId);
}
