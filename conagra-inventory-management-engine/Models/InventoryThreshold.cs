using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace conagra_inventory_management_engine.Models;

[Table("inventory_thresholds")]
public class InventoryThreshold : BaseModel
{
    [PrimaryKey("id")]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("threshold_quantity")]
    public int ThresholdQuantity { get; set; }
    
    // Navigation properties for related data
    public Store Store { get; set; }
    public Product Product { get; set; }
}

// Separate model for database insertion without navigation properties
[Table("inventory_thresholds")]
public class InventoryThresholdInsert : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("threshold_quantity")]
    public int ThresholdQuantity { get; set; }
    
    // No navigation properties
}
