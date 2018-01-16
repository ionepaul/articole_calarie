using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Converters
{
    public static class ProductConverter
    {
        public static Product ToDbProduct(this ProductViewModel productViewModel)
        {
            var product = new Product
            {

            };

            return product;
        }
    }
}
