using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class AddressRepository : AbstractRepository<Address>, IAddressRepository
    {
        public AddressRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }
    }
}
