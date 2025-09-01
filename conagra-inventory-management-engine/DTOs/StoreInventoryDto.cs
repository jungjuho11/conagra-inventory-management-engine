namespace conagra_inventory_management_engine.DTOs;

public class StoreInventoryDto
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? StoreName { get; set; }
    public string? ProductName { get; set; }
}

public class StoreInventoryDetailDto : StoreInventoryDto
{
    public int? ThresholdQuantity { get; set; }
    public bool IsBelowThreshold { get; set; }
}
