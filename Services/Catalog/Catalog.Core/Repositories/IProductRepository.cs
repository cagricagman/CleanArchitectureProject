using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecParams catalogSpecParams);
    Task<Product> GetProductByIdAsync(string id);
    Task<IEnumerable<Product>> GetProductsByName(string name);
    Task<IEnumerable<Product>> GetProductsByBrand(string brandName);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(string id);
}