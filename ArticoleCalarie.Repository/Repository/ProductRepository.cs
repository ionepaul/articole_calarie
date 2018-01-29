using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;
using ArticoleCalarie.Repository.Models;

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

                foreach (var colorId in product.ColorIds)
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
            var product = _dbset.Where(x => x.Id == productId)
                                .Include(x => x.Subcategory)
                                .Include(x => x.Subcategory.Category)
                                .Include(x => x.SizeChart)
                                .Include(x => x.Images)
                                .Include(x => x.AvailableColors)
                                .Include(x => x.Brand)
                                .FirstOrDefault();

            return product;
        }

        public ProductSearchResult GetProductsForAdmin(int itemsPerPage, int itemsToSkip, string productCode)
        {
            var query = _dbset.Include(x => x.Subcategory).Include(x => x.Brand);

            if (!string.IsNullOrEmpty(productCode))
            {
                var productSearchResult = new ProductSearchResult
                {
                    TotalCount = query.Where(x => x.ProductCode.StartsWith(productCode)).Count(),
                    Products = query.Where(x => x.ProductCode.StartsWith(productCode))
                                    .OrderBy(x => x.DatePosted).Skip(itemsToSkip).Take(itemsPerPage).AsEnumerable()
                };

                return productSearchResult;
            }

            var productResult = new ProductSearchResult
            {
                TotalCount = query.Count(),
                Products = query.OrderBy(x => x.DatePosted).Skip(itemsToSkip).Take(itemsPerPage).AsEnumerable()
            };

            return productResult;
        }

        public void UpdateProductCode(int productId, string productCode)
        {
            var product = _dbset.Find(productId);

            product.ProductCode = productCode;

            _ctx.Entry(product).State = EntityState.Modified;

            SaveChanges();
        }

        public async Task<ProductSearchResult> GetProductsBySearch(SearchModel searchModel)
        {
            var query = _dbset.Where(x => x.SubcategoryId == searchModel.SubcategoryId).Include(x => x.Images).OrderBy(x => x.DatePosted);

            var productSearchResult = new ProductSearchResult
            {
                TotalCount = await query.CountAsync(),
                Products = await query.Skip(searchModel.ItemsToSkip).Take(searchModel.ItemsPerPage).ToListAsync()
            };

            return productSearchResult;
        }
    }
}
