using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface IProductsService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(ProductQueryParameters queryParameters);
    Task<ProductDto?> GetProductByIdAsync(int productId);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
}
