using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IOrderLogic
    {
        Task<int> PlaceOrder(CheckoutViewModel checkoutViewModel, string userId);
    }
}
