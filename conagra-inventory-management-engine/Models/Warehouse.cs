using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace conagra_inventory_management_engine.Models;

[Table("warehouse")]
public class Warehouse : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    // Navigation property for related product data
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Product Product { get; set; }
}

// Separate model for database insertion without navigation properties
[Table("warehouse")]
public class WarehouseInsert : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    // No navigation properties
}
