﻿using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IProductLogic
    {
        void AddProduct(ProductViewModel product);
        ProductListAdminViewModel GetProductsForAdmin(int pageNumber, string productCode);
        Task<ProductSearchViewResult> GetProductsBySearch(SearchViewModel searchViewModel);
        ProductViewModel GetProductById(int id);
        void UpdateProduct(int id, ProductViewModel productViewModel);
        void DeleteProduct(int productId);
        ProductViewModel GetProductByProductCode(string productCode);
        SearchViewFilters GetSearchViewFiltersForSubcategory(int subcategoryId);
        Task<ProductSearchViewResult> GetProductsByBrand(string brand, int pageNumber);
        Task<ProductSearchViewResult> GetProductsOnSale(int pageNumber);
    }
}
