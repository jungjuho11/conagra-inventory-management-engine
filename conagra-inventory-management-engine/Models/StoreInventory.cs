using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace conagra_inventory_management_engine.Models;

[Table("store_inventory")]
public class StoreInventory : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    // Navigation properties for related data
    public Store Store { get; set; }
    public Product Product { get; set; }
}

// Separate model for database insertion without navigation properties
[Table("store_inventory")]
public class StoreInventoryInsert : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    // No navigation properties
}
