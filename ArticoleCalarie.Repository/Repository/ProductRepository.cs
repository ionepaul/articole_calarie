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

            SaveChanges();
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

        public Product GetProductByProductCode(string productCode)
        {
            var product = _dbset.Where(x => x.ProductCode == productCode)
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
                    Products = query.Where(x => x.ProductCode.StartsWith(productCode) || x.ProductName.StartsWith(productCode))
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
            var query = _dbset.Where(x => x.SubcategoryId == searchModel.SubcategoryId)
                              .Include(x => x.Images);

            if (searchModel.MinPrice.HasValue && searchModel.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price >= searchModel.MinPrice.Value &&  x.Price <= searchModel.MaxPrice.Value);
            }

            if (searchModel.ColorIds != null && searchModel.ColorIds.Count > 0)
            {
                query = query.Where(x => searchModel.ColorIds.Intersect(x.AvailableColors.Select(y => y.Id)).Count() > 0);
            }

            if (searchModel.Sizes != null && searchModel.Sizes.Count > 0)
            {
                var sizeQuery = query.Select(x => new { x.Id, x.Size }).ToList();
                var productIds = new List<int>();

                foreach(var product in sizeQuery)
                {
                    if (searchModel.Sizes.Intersect(product.Size.Split(',').ToList()).Count() > 0)
                    {
                        productIds.Add(product.Id);
                    }
                }

                query = query.Where(x => productIds.Contains(x.Id));
            }

            query = query.OrderBy(x => x.DatePosted);

            var productSearchResult = new ProductSearchResult
            {
                TotalCount = await query.CountAsync(),
                Products = await query.Skip(searchModel.ItemsToSkip).Take(searchModel.ItemsPerPage).ToListAsync()
            };

            return productSearchResult;
        }

        public void UpdateProduct(Product product)
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

            _ctx.Entry(product).State = EntityState.Modified;

            SaveChanges();
        }

        public SearchFilters GetSearchFiltersForSubcategory(int subcategoryId)
        {
            var subcategoryProducts = _dbset.Where(x => x.SubcategoryId == subcategoryId).Include(x => x.AvailableColors);

            var searchFilters = new SearchFilters
            {
                MinPrice = subcategoryProducts.Min(p => p.Price),
                MaxPrice = subcategoryProducts.Max(p => p.Price)
            };

            var productsColors = subcategoryProducts.Where(x => x.AvailableColors.Count > 0)
                                            .SelectMany(x => x.AvailableColors).Select(y => new ColorDTO { Id = y.Id, HexValue = y.HexValue }).Distinct();

            searchFilters.Colors = productsColors.AsEnumerable();

            var sizes = subcategoryProducts.Select(x => x.Size).Distinct().ToList();

            var productsSizes = new List<string>();

            if (sizes != null && sizes.Count() > 0)
            {
                foreach (var sizeList in sizes)
                {
                    if (sizeList != null)
                    {
                        var itemSizes = sizeList.Split(',');

                        productsSizes.AddRange(itemSizes);
                    }
                }

                searchFilters.Sizes = productsSizes.Distinct().Where(x => !string.IsNullOrEmpty(x));
            }
  
            return searchFilters;
        }

        public async Task<ProductSearchResult> GetProductsByBrand(string brand, int itemsPerPage, int itemsToSkip)
        {
            var query = _dbset.Include(x => x.Brand).Where(x => string.Equals(x.Brand.Name, brand));

            var productResult = new ProductSearchResult
            {
                TotalCount = await query.CountAsync(),
                Products = await query.OrderBy(x => x.SalePercentage).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
            };

            return productResult;
        }

        public async Task<ProductSearchResult> GetProductsOnSale(int itemsPerPage, int itemsToSkip)
        {
            var query = _dbset.Where(x => x.SalePercentage != 0);

            var productResult = new ProductSearchResult
            {
                TotalCount = await query.CountAsync(),
                Products = await query.OrderBy(x => x.SalePercentage).Skip(itemsToSkip).Take(itemsPerPage).ToListAsync()
            };

            return productResult;
        }
    }
}
