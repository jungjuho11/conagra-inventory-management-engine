using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductByNameAsync(string name);
    Task<int> GetLastProductIdAsync();
    Task<Product> CreateProductAsync(Product product);
}
