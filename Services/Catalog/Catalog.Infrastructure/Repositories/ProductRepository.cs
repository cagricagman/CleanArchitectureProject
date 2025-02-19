using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository: IProductRepository,ITypesRepository,IBrandRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }
    public async Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
        if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            var brandFilter = builder.Eq(p=>p.Brands.Id, catalogSpecParams.BrandId);
            filter &= brandFilter;
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            var typeFilter = builder.Eq(p=>p.Types.Id, catalogSpecParams.TypeId);
            filter &= typeFilter;
        }
        var totalItems = await _context.Products.CountDocumentsAsync(filter);
        var data = await DataFilter(catalogSpecParams, filter);
        return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageSize, (int)totalItems, data);
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        var sortDef = Builders<Product>.Sort.Ascending("Name");
        if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            switch (catalogSpecParams.Sort)
            {
                case "priceAsc":
                    sortDef = Builders<Product>.Sort.Ascending(p=>p.Price);
                    break;
                case "priceDesc":
                    sortDef = Builders<Product>.Sort.Descending(p=>p.Price);
                    break;
                default:
                    sortDef = Builders<Product>.Sort.Ascending(p=>p.Name);
                    break;
            }
        }
        return await _context.Products
            .Find(filter)
            .Sort(sortDef)
            .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
            .Limit(catalogSpecParams.PageSize)
            .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return await _context.Products.Find(p => p.Name.ToLower() == name.ToLower()).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
    {
        return await _context.Products.Find(p=>p.Brands.Name.ToLower() == brandName.ToLower()).ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var updateResult = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var deleteResult = await _context.Products.DeleteOneAsync(p => p.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductType>> GetAllTypesAsync()
    {
        return await _context.Types.Find(x => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync()
    {
        return await _context.Brands.Find(x => true).ToListAsync();
    }
}