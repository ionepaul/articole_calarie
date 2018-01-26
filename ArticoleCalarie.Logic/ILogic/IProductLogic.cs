using System.Collections.Generic;
using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IProductLogic
    {
        void AddProduct(ProductViewModel product);
        IEnumerable<ProductListItemModel> GetProductsList();
        Task<ProductSearchViewResult> GetProductsBySearch(SearchViewModel searchViewModel);
    }
}
