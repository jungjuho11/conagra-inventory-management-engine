using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly Supabase.Client _supabaseClient;

    public StoreRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<Store>> GetAllStoresAsync()
    {
        var response = await _supabaseClient
            .From<Store>()
            .Get();
        
        return response.Models;
    }

    public async Task<Store?> GetStoreByIdAsync(int id)
    {
        var response = await _supabaseClient
            .From<Store>()
            .Where(x => x.Id == id)
            .Get();
        
        return response.Models.FirstOrDefault();
    }
}
