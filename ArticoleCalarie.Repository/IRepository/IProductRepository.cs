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
        IEnumerable<Product> GetProducts();
        Task<ProductSearchResult> GetProductsBySearch(SearchModel searchModel);
    }
}
