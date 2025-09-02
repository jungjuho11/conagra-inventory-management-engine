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
        
        var storeInventories = response.Models.ToList();
        
        // Load store and product information for each inventory item
        foreach (var inventory in storeInventories)
        {
            // Load store information
            if (inventory.StoreId > 0)
            {
                var storeResponse = await _supabaseClient
                    .From<Store>()
                    .Where(x => x.Id == inventory.StoreId)
                    .Get();
                
                inventory.Store = storeResponse.Models.FirstOrDefault();
            }
            
            // Load product information
            if (inventory.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == inventory.ProductId)
                    .Get();
                
                inventory.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        return storeInventories;
    }

    public async Task<IEnumerable<StoreInventory>> GetStoreInventoryByStoreIdAsync(int storeId)
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Where(x => x.StoreId == storeId)
            .Get();
        
        var storeInventories = response.Models.ToList();
        
        // Load store and product information for each inventory item
        foreach (var inventory in storeInventories)
        {
            // Load store information
            if (inventory.StoreId > 0)
            {
                var storeResponse = await _supabaseClient
                    .From<Store>()
                    .Where(x => x.Id == inventory.StoreId)
                    .Get();
                
                inventory.Store = storeResponse.Models.FirstOrDefault();
            }
            
            // Load product information
            if (inventory.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == inventory.ProductId)
                    .Get();
                
                inventory.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        return storeInventories;
    }

    public async Task<StoreInventory?> GetStoreInventoryByStoreAndProductAsync(int storeId, int productId)
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Where(x => x.StoreId == storeId && x.ProductId == productId)
            .Get();
        
        var inventory = response.Models.FirstOrDefault();
        
        // Load store and product information
        if (inventory != null)
        {
            // Load store information
            if (inventory.StoreId > 0)
            {
                var storeResponse = await _supabaseClient
                    .From<Store>()
                    .Where(x => x.Id == inventory.StoreId)
                    .Get();
                
                inventory.Store = storeResponse.Models.FirstOrDefault();
            }
            
            // Load product information
            if (inventory.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == inventory.ProductId)
                    .Get();
                
                inventory.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        return inventory;
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

    public async Task<int> GetLastStoreInventoryIdAsync()
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Descending)
            .Limit(1)
            .Get();
        
        if (response.Models.Any())
        {
            return response.Models.First().Id;
        }
        
        return 0; // If no store inventory records exist, start with ID 1
    }

    public async Task<StoreInventory> CreateStoreInventoryAsync(StoreInventory storeInventory)
    {
        // Use StoreInventoryInsert model to avoid navigation property issues
        var storeInventoryToInsert = new StoreInventoryInsert
        {
            Id = storeInventory.Id,
            StoreId = storeInventory.StoreId,
            ProductId = storeInventory.ProductId,
            Quantity = storeInventory.Quantity
        };
        
        var response = await _supabaseClient
            .From<StoreInventoryInsert>()
            .Insert(storeInventoryToInsert);
        
        // Convert back to StoreInventory model for return
        var insertedStoreInventory = response.Models.First();
        return new StoreInventory
        {
            Id = insertedStoreInventory.Id,
            StoreId = insertedStoreInventory.StoreId,
            ProductId = insertedStoreInventory.ProductId,
            Quantity = insertedStoreInventory.Quantity
        };
    }

    public async Task<StoreInventory> UpdateStoreInventoryQuantityAsync(int storeId, int productId, int newQuantity)
    {
        var response = await _supabaseClient
            .From<StoreInventory>()
            .Where(x => x.StoreId == storeId && x.ProductId == productId)
            .Set(x => x.Quantity, newQuantity)
            .Get();
    
        return response.Models.First();
    }
}
