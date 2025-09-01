using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;

namespace conagra_inventory_management_engine.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<IEnumerable<Warehouse>> GetWarehouseInventoryAsync()
    {
        return await _warehouseRepository.GetAllWarehouseInventoryAsync();
    }

    public async Task<Warehouse?> GetWarehouseInventoryByProductAsync(int productId)
    {
        return await _warehouseRepository.GetWarehouseInventoryByProductIdAsync(productId);
    }
}
