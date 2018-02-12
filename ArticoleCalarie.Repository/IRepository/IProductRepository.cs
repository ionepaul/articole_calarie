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
    }
}
