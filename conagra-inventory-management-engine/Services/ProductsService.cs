using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;

namespace conagra_inventory_management_engine.Services;

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;

    public ProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllProductsAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await _productRepository.GetProductByIdAsync(productId);
    }
}
