using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Repositories;

public interface IWarehouseRepository
{
    Task<IEnumerable<Warehouse>> GetAllWarehouseInventoryAsync();
    Task<Warehouse?> GetWarehouseInventoryByProductIdAsync(int productId);
    Task<Warehouse?> GetWarehouseInventoryByProductNameAsync(string productName);
    Task<bool> UpdateWarehouseQuantityAsync(int productId, int newQuantity);
}
