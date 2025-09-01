using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public interface IProductsService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(QueryParameters queryParameters);
    Task<ProductDto?> GetProductByIdAsync(int productId);
}
