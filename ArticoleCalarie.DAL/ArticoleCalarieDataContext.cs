using System.Data.Entity;
using ArticoleCalarie.Repository.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArticoleCalarie.Repository
{
    public class ArticoleCalarieDataContext : IdentityDbContext<UserModel>
    {
        public ArticoleCalarieDataContext() : base("ArticoleCalarieDataContext") { }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Color> Colors { get; set; }

        public static ArticoleCalarieDataContext Create()
        {
            return new ArticoleCalarieDataContext();
        }
    }
}
