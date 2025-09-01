using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

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
    public Product Product { get; set; }
}
