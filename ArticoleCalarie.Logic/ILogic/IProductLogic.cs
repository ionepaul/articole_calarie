using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IProductLogic
    {
        void AddProduct(ProductViewModel product);
        Task<ProductListAdminViewModel> GetProductsForAdmin(int pageNumber, string productCode);
        Task<ProductSearchViewResult> GetProductsBySearch(SearchViewModel searchViewModel);
    }
}
