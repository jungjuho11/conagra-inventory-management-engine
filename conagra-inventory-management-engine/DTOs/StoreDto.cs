namespace conagra_inventory_management_engine.DTOs;

public class StoreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
}

public class StoreDetailDto : StoreDto
{
    // Add any additional fields for detailed store view if needed
}
