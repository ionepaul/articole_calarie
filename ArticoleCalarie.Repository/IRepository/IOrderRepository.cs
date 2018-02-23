using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IOrderRepository : IAbstractRepository<Order>
    {
        Task<int> CountOrders();
    }
}
