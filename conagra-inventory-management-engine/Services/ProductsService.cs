using conagra_inventory_management_engine.Models;
using conagra_inventory_management_engine.Repositories;
using conagra_inventory_management_engine.Common;
using conagra_inventory_management_engine.DTOs;

namespace conagra_inventory_management_engine.Services;

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;

    public ProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductQueryParameters queryParameters)
    {
        var products = await _productRepository.GetAllProductsAsync();
        
        // Apply product ID filter
        if (queryParameters.ProductId.HasValue)
        {
            products = products.Where(p => p.Id == queryParameters.ProductId.Value);
        }
        
        // Apply product name filter (partial match)
        if (!string.IsNullOrEmpty(queryParameters.ProductName))
        {
            products = products.Where(p => p.Name.Contains(queryParameters.ProductName, StringComparison.OrdinalIgnoreCase));
        }
        
        // Apply search filter
        if (!string.IsNullOrEmpty(queryParameters.Search))
        {
            products = products.Where(p => p.Name.Contains(queryParameters.Search, StringComparison.OrdinalIgnoreCase));
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            products = queryParameters.SortBy.ToLower() switch
            {
                "name" => queryParameters.SortOrder.ToLower() == "desc" 
                    ? products.OrderByDescending(p => p.Name)
                    : products.OrderBy(p => p.Name),
                "id" => queryParameters.SortOrder.ToLower() == "desc"
                    ? products.OrderByDescending(p => p.Id)
                    : products.OrderBy(p => p.Id),
                _ => products.OrderBy(p => p.Id)
            };
        }
        else
        {
            products = products.OrderBy(p => p.Id);
        }

        var totalCount = products.Count();
        var pagedProducts = products
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(p => new ProductDto { Id = p.Id, Name = p.Name });

        return new PagedResult<ProductDto>
        {
            Data = pagedProducts,
            Pagination = new PaginationInfo
            {
                Page = queryParameters.Page,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount
            }
        };
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        var product = await _productRepository.GetProductByIdAsync(productId);
        return product == null ? null : new ProductDto { Id = product.Id, Name = product.Name };
    }
}
