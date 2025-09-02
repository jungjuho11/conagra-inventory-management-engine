namespace conagra_inventory_management_engine.DTOs;

public class RefillNotificationDto
{
    public int StoreId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreAddress { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int ThresholdQuantity { get; set; }
    public int QuantityNeeded { get; set; }
    public bool IsUrgent { get; set; } // True if current quantity is 0 or below threshold
}
