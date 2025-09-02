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

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        var response = await _supabaseClient
            .From<Product>()
            .Get();
        
        // Filter in memory for case-insensitive comparison
        return response.Models
            .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<int> GetLastProductIdAsync()
    {
        var response = await _supabaseClient
            .From<Product>()
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Descending)
            .Limit(1)
            .Get();
        
        if (response.Models.Any())
        {
            return response.Models.First().Id;
        }
        
        return 0; // If no products exist, start with ID 1
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        var response = await _supabaseClient
            .From<Product>()
            .Insert(product);
        
        return response.Models.First();
    }
}
