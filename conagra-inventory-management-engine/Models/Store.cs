using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace conagra_inventory_management_engine.Models;

[Table("stores")]
public class Store : BaseModel
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("address")]
    public string? Address { get; set; }
}
