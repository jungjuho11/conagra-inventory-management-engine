using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Services;

public class InventoryThresholdsService : IInventoryThresholdsService
{
    private readonly Supabase.Client _supabaseClient;

    public InventoryThresholdsService(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<InventoryThreshold>> GetAllInventoryThresholdsAsync()
    {
        var response = await _supabaseClient.From<InventoryThreshold>().Get();
        return response.Models;
    }

    public async Task<IEnumerable<InventoryThreshold>> GetInventoryThresholdsByStoreAsync(int storeId)
    {
        var response = await _supabaseClient.From<InventoryThreshold>().Where(x => x.StoreId == storeId).Get();
        return response.Models;
    }

    public async Task<IEnumerable<InventoryThreshold>> GetInventoryThresholdsByProductAsync(int productId)
    {
        var response = await _supabaseClient.From<InventoryThreshold>().Where(x => x.ProductId == productId).Get();
        return response.Models;
    }

    public async Task<InventoryThreshold?> GetInventoryThresholdByStoreAndProductAsync(int storeId, int productId)
    {
        var response = await _supabaseClient.From<InventoryThreshold>().Where(x => x.StoreId == storeId && x.ProductId == productId).Get();
        return response.Models.FirstOrDefault();
    }
}
