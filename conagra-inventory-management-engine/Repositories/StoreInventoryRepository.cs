using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class StoreInventoryRepository : IStoreInventoryRepository
{
    private readonly Supabase.Client _supabaseClient;

    public StoreInventoryRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<StoreInventory>> GetAllStoresInventoryAsync()
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Get();
        
        return response.Models;
    }

    public async Task<IEnumerable<StoreInventory>> GetStoreInventoryByStoreIdAsync(int storeId)
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Where(x => x.StoreId == storeId)
            .Get();
        
        return response.Models;
    }

    public async Task<StoreInventory?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId)
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Where(x => x.StoreId == storeId && x.ProductId == productId)
            .Get();
        
        return response.Models.FirstOrDefault();
    }

    public async Task<StoreInventory> ShipProductToStoreAsync(int storeId, int productId, int quantity)
    {
        // First, check if the store already has this product
        var existingInventory = await GetStoreInventoryByStoreAndProductAsync(storeId, productId);
    
        if (existingInventory != null)
        {
            // Update existing inventory - search by store_id and product_id, not by id
            var response = await _supabaseClient
                .From<StoreInventory>()
                .Where(x => x.StoreId == storeId && x.ProductId == productId)
                .Set(x => x.Quantity, existingInventory.Quantity + quantity)
                .Get();
        
            return response.Models.First();
        }
        else
        {
            // Create new inventory record
            var newInventory = new StoreInventory
            {
                StoreId = storeId,
                ProductId = productId,
                Quantity = quantity,
            };
        
            var response = await _supabaseClient
                .From<StoreInventory>()
                .Insert(newInventory);
        
            return response.Models.First();
        }
    }

    public async Task<IEnumerable<StoreInventory>> GetStoresBelowThresholdAsync()
    {
        // Get all store inventory records
        var allInventory = await GetAllStoresInventoryAsync();
        var storesBelowThreshold = new List<StoreInventory>();

        foreach (var inventory in allInventory)
        {
            // Get the threshold for this store and product
            var thresholdResponse = await _supabaseClient
                .From<InventoryThreshold>()
                .Where(x => x.StoreId == inventory.StoreId && x.ProductId == inventory.ProductId)
                .Get();

            var threshold = thresholdResponse.Models.FirstOrDefault();
            
            // If threshold exists and current quantity is below it, add to the list
            if (threshold != null && inventory.Quantity < threshold.ThresholdQuantity)
            {
                storesBelowThreshold.Add(inventory);
            }
        }

        return storesBelowThreshold;
    }
}
