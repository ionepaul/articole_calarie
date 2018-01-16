using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IProductLogic
    {
        Task AddProduct(ProductViewModel product);
    }
}
