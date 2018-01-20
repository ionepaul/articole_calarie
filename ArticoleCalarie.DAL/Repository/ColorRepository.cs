using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class ColorRepository : AbstractRepository<Color>, IColorRepository
    {
        public ColorRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }
    }
}
