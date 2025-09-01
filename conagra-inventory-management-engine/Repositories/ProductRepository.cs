using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly Supabase.Client _supabaseClient;

    public ProductRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var response = await _supabaseClient
            .From<Product>()
            .Get();
        
        return response.Models;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var response = await _supabaseClient
            .From<Product>()
            .Where(x => x.Id == id)
            .Get();
        
        return response.Models.FirstOrDefault();
    }
}
