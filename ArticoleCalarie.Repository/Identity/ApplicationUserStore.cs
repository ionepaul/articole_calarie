using System.Data.Entity;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArticoleCalarie.Repository.Identity
{
    public class ApplicationUserStore : UserStore<UserModel>
    {
        public ApplicationUserStore(ArticoleCalarieDataContext context) : base(context)
        {
        }

        public override async Task<UserModel> FindByIdAsync(string userId)
        {
            return await Users.Include(u => u.Roles).Include(u => u.DeliveryAddress).Include(u => u.BillingAddress).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
