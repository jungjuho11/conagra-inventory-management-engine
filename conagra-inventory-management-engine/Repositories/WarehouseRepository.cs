using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly Supabase.Client _supabaseClient;

    public WarehouseRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<Warehouse>> GetAllWarehouseInventoryAsync()
    {
        var response = await _supabaseClient
            .From<Warehouse>()
            .Get();
        
        return response.Models;
    }

    public async Task<Warehouse?> GetWarehouseInventoryByProductIdAsync(int productId)
    {
        var response = await _supabaseClient
            .From<Warehouse>()
            .Where(x => x.ProductId == productId)
            .Get();
        
        return response.Models.FirstOrDefault();
    }

    public async Task<bool> UpdateWarehouseQuantityAsync(int productId, int newQuantity)
    {
        var response = await _supabaseClient
            .From<Warehouse>()
            .Where(x => x.ProductId == productId)
            .Set(x => x.Quantity, newQuantity)
            .Update();
    
        return response.Models.Count > 0;
    }
}
