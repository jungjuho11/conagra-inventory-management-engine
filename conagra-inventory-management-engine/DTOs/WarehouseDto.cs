namespace conagra_inventory_management_engine.DTOs;

public class WarehouseDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? ProductName { get; set; }
}

public class WarehouseDetailDto : WarehouseDto
{
    // Add any additional fields for detailed warehouse view if needed
}
