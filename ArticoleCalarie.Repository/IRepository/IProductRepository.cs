using System.Collections.Generic;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Models;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IProductRepository : IAbstractRepository<Product>
    {
        Product GetProductById(int productId);
        void UpdateProductCode(int productId, string productCode);
        void AddProductToDb(Product product);
        Task<ProductSearchResult> GetProductsBySearch(SearchModel searchModel);
        ProductSearchResult GetProductsForAdmin(int itemsPerPage, int itemsToSkip, string productCode);
        void UpdateProduct(Product product);
        Product GetProductByProductCode(string productCode);
        SearchFilters GetSearchFiltersForSubcategory(int subcategoryId);
        Task<ProductSearchResult> GetProductsByBrand(string brand, int itemsPerPage, int itemsToSkip);
        Task<ProductSearchResult> GetProductsOnSale(int itemsPerPage, int itemsToSkip);
        Task<IEnumerable<Product>> GetRelatedProducts(string subcategory);
        Task<IEnumerable<Product>> GetTheNewestProductsForHome(int daysToKeepProductNew);
        Task<IEnumerable<Product>> GetProducstOnSaleForHome();
        Task<ProductSearchResult> GetTheNewestProducts(int itemsPerPage, int itemsToSkip, int daysToKeepProductMarkedNew);
    }
}
