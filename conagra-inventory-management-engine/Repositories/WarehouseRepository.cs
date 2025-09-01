using conagra_inventory_management_engine.Models;
using Supabase;

namespace conagra_inventory_management_engine.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly Supabase.Client _supabaseClient;

    public WarehouseRepository(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<IEnumerable<Warehouse>> GetAllWarehouseInventoryAsync()
    {
        var response = await _supabaseClient
            .From<Warehouse>()
            .Get();
        
        var warehouses = response.Models.ToList();
        
        // Load product information for each warehouse item
        foreach (var warehouse in warehouses)
        {
            if (warehouse.ProductId > 0)
            {
                var productResponse = await _supabaseClient
                    .From<Product>()
                    .Where(x => x.Id == warehouse.ProductId)
                    .Get();
                
                warehouse.Product = productResponse.Models.FirstOrDefault();
            }
        }
        
        return warehouses;
    }

    public async Task<Warehouse?> GetWarehouseInventoryByProductIdAsync(int productId)
    {
        var response = await _supabaseClient
            .From<Warehouse>()
            .Where(x => x.ProductId == productId)
            .Get();
        
        var warehouse = response.Models.FirstOrDefault();
        
        // Load product information
        if (warehouse != null && warehouse.ProductId > 0)
        {
            var productResponse = await _supabaseClient
                .From<Product>()
                .Where(x => x.Id == warehouse.ProductId)
                .Get();
            
            warehouse.Product = productResponse.Models.FirstOrDefault();
        }
        
        return warehouse;
    }

    public async Task<Warehouse?> GetWarehouseInventoryByProductNameAsync(string productName)
    {
        // First, get all products and filter by name (case-insensitive partial match)
        var productResponse = await _supabaseClient
            .From<Product>()
            .Get();
        
        var product = productResponse.Models
            .FirstOrDefault(p => p.Name.Contains(productName, StringComparison.OrdinalIgnoreCase));
        
        if (product == null)
            return null;
        
        // Then find the warehouse inventory for that product
        var warehouseResponse = await _supabaseClient
            .From<Warehouse>()
            .Where(x => x.ProductId == product.Id)
            .Get();
        
        var warehouse = warehouseResponse.Models.FirstOrDefault();
        
        // Load product information
        if (warehouse != null)
        {
            warehouse.Product = product;
        }
        
        return warehouse;
    }

    public async Task<bool> UpdateWarehouseQuantityAsync(int productId, int newQuantity)
    {
        var response = await _supabaseClient
            .From<Warehouse>()
            .Where(x => x.ProductId == productId)
            .Set(x => x.Quantity, newQuantity)
            .Update();
    
        return response.Models.Count > 0;
    }
}
