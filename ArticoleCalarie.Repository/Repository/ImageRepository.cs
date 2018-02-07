using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Repository.Repository
{
    public class ImageRepository : AbstractRepository<Image>, IImageRepository
    {
        public ImageRepository(ArticoleCalarieDataContext dataContext) : base(dataContext)
        {
        }
    }
}
