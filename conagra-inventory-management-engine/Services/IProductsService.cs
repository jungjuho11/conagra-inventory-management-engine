using conagra_inventory_management_engine.Models;

namespace conagra_inventory_management_engine.Services;

public interface IProductsService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int productId);
}
