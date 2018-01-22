using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class ProductRepository : AbstractRepository<Product>, IProductRepository
    {
        public ProductRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }

        public void AddProductToDb(Product product)
        {
            if (product.ColorIds != null)
            {
                product.AvailableColors = new List<Color>();

                foreach(var colorId in product.ColorIds)
                {
                    var color = _ctx.Colors.Find(colorId);

                    product.AvailableColors.Add(color);
                }
            }

            _dbset.Add(product);

            _ctx.SaveChanges();
        }

        public Product GetProductById(int productId)
        {
            var product = _dbset.Where(x => x.Id == productId).Include(x => x.Subcategory).Include(x => x.Subcategory.Category).FirstOrDefault();

            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _dbset.Include(x => x.Subcategory).Include(x => x.Brand).AsEnumerable();

            return products;
        }

        public void UpdateProductCode(int productId, string productCode)
        {
            var product = _dbset.Find(productId);

            product.ProductCode = productCode;

            _ctx.Entry(product).State = EntityState.Modified;

            SaveChanges();
        }
    }
}
