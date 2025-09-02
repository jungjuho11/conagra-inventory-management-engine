using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly Supabase.Client _supabaseClient;

    public NotificationRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<RefillNotificationData>> GetRefillNotificationsAsync()
    {
        // Get all inventory thresholds
        var thresholdsResponse = await _supabaseClient
            .From<InventoryThreshold>()
            .Get();
        
        var thresholds = thresholdsResponse.Models.ToList();
        var refillNotifications = new List<RefillNotificationData>();

        foreach (var threshold in thresholds)
        {
            // Get store information
            var storeResponse = await _supabaseClient
                .From<Store>()
                .Where(x => x.Id == threshold.StoreId)
                .Get();
            
            var store = storeResponse.Models.FirstOrDefault();
            if (store == null) continue;

            // Get product information
            var productResponse = await _supabaseClient
                .From<Product>()
                .Where(x => x.Id == threshold.ProductId)
                .Get();
            
            var product = productResponse.Models.FirstOrDefault();
            if (product == null) continue;

            // Get current store inventory for this product
            var storeInventoryResponse = await _supabaseClient
                .From<StoreInventory>()
                .Where(x => x.StoreId == threshold.StoreId && x.ProductId == threshold.ProductId)
                .Get();
            
            var storeInventory = storeInventoryResponse.Models.FirstOrDefault();
            var currentQuantity = storeInventory?.Quantity ?? 0;

            // Check if refill is needed (current quantity <= threshold)
            if (currentQuantity <= threshold.ThresholdQuantity)
            {
                var quantityNeeded = threshold.ThresholdQuantity - currentQuantity;
                var isUrgent = currentQuantity == 0 || currentQuantity <= threshold.ThresholdQuantity; // Urgent if 0 or at/below threshold

                refillNotifications.Add(new RefillNotificationData
                {
                    StoreId = store.Id,
                    StoreName = store.Name,
                    StoreAddress = store.Address ?? string.Empty,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    CurrentQuantity = currentQuantity,
                    ThresholdQuantity = threshold.ThresholdQuantity,
                    QuantityNeeded = quantityNeeded,
                    IsUrgent = isUrgent
                });
            }
        }

        return refillNotifications;
    }
}

// Helper class to hold the data before converting to DTO
public class RefillNotificationData
{
    public int StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreAddress { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int ThresholdQuantity { get; set; }
    public int QuantityNeeded { get; set; }
    public bool IsUrgent { get; set; }
}
