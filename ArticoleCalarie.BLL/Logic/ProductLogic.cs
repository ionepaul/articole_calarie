using System.Threading.Tasks;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class ProductLogic : IProductLogic
    {
        private IProductRepository _iProductRepository;

        public ProductLogic(IProductRepository iProductRepository)
        {
            _iProductRepository = iProductRepository;
        }

        public async Task AddProduct(ProductViewModel productViewModel)
        {
            var product = productViewModel.ToDbProduct();

            await _iProductRepository.Add(product);
        }
    }
}
