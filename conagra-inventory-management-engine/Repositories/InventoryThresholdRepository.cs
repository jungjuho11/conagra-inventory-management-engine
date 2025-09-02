using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class InventoryThresholdRepository : IInventoryThresholdRepository
{
    private readonly Supabase.Client _supabaseClient;

    public InventoryThresholdRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<InventoryThreshold>> GetAllInventoryThresholdsAsync()
    {
        var response = await _supabaseClient
            .From<InventoryThreshold>()
            .Get();
        
        var inventoryThresholds = response.Models.ToList();
        
        // Load store and product information for each threshold
        foreach (var threshold in inventoryThresholds)
        {
            // Load store information
            if (threshold.StoreId > 0)
            {
                var storeResponse = await _supabaseClient
                    .From<Store>()
                    .Where(x => x.Id == threshold.StoreId)
                    .Get();
                
                threshold.Store = storeResponse.Models.FirstOrDefault();
            }
            
            // Load product information
            if (threshold.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == threshold.ProductId)
                    .Get();
                
                threshold.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        return inventoryThresholds;
    }

    public async Task<InventoryThreshold?> GetInventoryThresholdByStoreAndProductAsync(int storeId, int productId)
    {
        var response = await _supabaseClient
            .From<InventoryThreshold>()
            .Where(x => x.StoreId == storeId && x.ProductId == productId)
            .Get();
        
        var threshold = response.Models.FirstOrDefault();
        
        // Load store and product information
        if (threshold != null)
        {
            // Load store information
            if (threshold.StoreId > 0)
            {
                var storeResponse = await _supabaseClient
                    .From<Store>()
                    .Where(x => x.Id == threshold.StoreId)
                    .Get();
                
                threshold.Store = storeResponse.Models.FirstOrDefault();
            }
            
            // Load product information
            if (threshold.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == threshold.ProductId)
                    .Get();
                
                threshold.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        return threshold;
    }

    public async Task<bool> StoreExistsAsync(int storeId)
    {
        var response = await _supabaseClient
            .From<Store>()
            .Where(x => x.Id == storeId)
            .Get();
        
        return response.Models.Any();
    }

    public async Task<bool> ProductExistsAsync(int productId)
    {
        var response = await _supabaseClient
            .From<Product>()
            .Where(x => x.Id == productId)
            .Get();
        
        return response.Models.Any();
    }

    public async Task<int> GetLastInventoryThresholdIdAsync()
    {
        var response = await _supabaseClient
            .From<InventoryThreshold>()
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Descending)
            .Limit(1)
            .Get();
        
        if (response.Models.Any())
        {
            return response.Models.First().Id;
        }
        
        return 0; // If no inventory threshold records exist, start with ID 1
    }

    public async Task<InventoryThreshold> CreateInventoryThresholdAsync(InventoryThreshold inventoryThreshold)
    {
        // Use InventoryThresholdInsert model to avoid navigation property issues
        var thresholdToInsert = new InventoryThresholdInsert
        {
            Id = inventoryThreshold.Id,
            StoreId = inventoryThreshold.StoreId,
            ProductId = inventoryThreshold.ProductId,
            ThresholdQuantity = inventoryThreshold.ThresholdQuantity
        };
        
        var response = await _supabaseClient
            .From<InventoryThresholdInsert>()
            .Insert(thresholdToInsert);
        
        // Convert back to InventoryThreshold model for return
        var insertedThreshold = response.Models.First();
        return new InventoryThreshold
        {
            Id = insertedThreshold.Id,
            StoreId = insertedThreshold.StoreId,
            ProductId = insertedThreshold.ProductId,
            ThresholdQuantity = insertedThreshold.ThresholdQuantity
        };
    }

    public async Task<InventoryThreshold> UpdateInventoryThresholdQuantityAsync(int storeId, int productId, int newThresholdQuantity)
    {
        var response = await _supabaseClient
            .From<InventoryThreshold>()
            .Where(x => x.StoreId == storeId && x.ProductId == productId)
            .Set(x => x.ThresholdQuantity, newThresholdQuantity)
            .Update();
    
        return response.Models.First();
    }
}
