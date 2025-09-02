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

    public async Task<int> GetLastStoreIdAsync()
    {
        var response = await _supabaseClient
            .From<Store>()
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Descending)
            .Limit(1)
            .Get();
        
        if (response.Models.Any())
        {
            return response.Models.First().Id;
        }
        
        return 0; // If no stores exist, start with ID 1
    }

    public async Task<Store> CreateStoreAsync(Store store)
    {
        var response = await _supabaseClient
            .From<Store>()
            .Insert(store);
        
        return response.Models.First();
    }
}
