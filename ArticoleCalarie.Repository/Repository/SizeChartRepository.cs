using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class SizeChartRepository : AbstractRepository<SizeChart>, ISizeChartRepository
    {
        public SizeChartRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }
    }
}
