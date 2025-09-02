namespace conagra_inventory_management_engine.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
}

public class ProductDetailDto : ProductDto
{
    // Add any additional fields for detailed product view if needed
}
