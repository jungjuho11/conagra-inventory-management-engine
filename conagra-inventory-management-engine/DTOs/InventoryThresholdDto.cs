namespace conagra_inventory_management_engine.DTOs;

public class InventoryThresholdDto
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public int ThresholdQuantity { get; set; }
    public string? StoreName { get; set; }
    public string? ProductName { get; set; }
}

public class CreateInventoryThresholdDto
{
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public int ThresholdQuantity { get; set; }
}

public class InventoryThresholdDetailDto : InventoryThresholdDto
{
    public int? CurrentQuantity { get; set; }
    public bool IsBelowThreshold { get; set; }
}
