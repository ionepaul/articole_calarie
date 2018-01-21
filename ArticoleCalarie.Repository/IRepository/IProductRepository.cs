using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IProductRepository : IAbstractRepository<Product>
    {
        void UpdateProductCode(int productId, string productCode);
    }
}
