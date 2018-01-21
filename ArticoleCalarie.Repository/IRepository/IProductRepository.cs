using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IProductRepository : IAbstractRepository<Product>
    {
        Product GetProductById(int productId);
        void UpdateProductCode(int productId, string productCode);
        void AddProductToDb(Product product);
    }
}
