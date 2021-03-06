﻿using System.Data.Entity;
using ArticoleCalarie.Repository.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArticoleCalarie.Repository
{
    public class ArticoleCalarieDataContext : IdentityDbContext<UserModel>
    {
        public ArticoleCalarieDataContext() : base("ArticoleCalarieDataContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }

        public DbSet<SizeChart> SizeCharts { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public static ArticoleCalarieDataContext Create()
        {
            return new ArticoleCalarieDataContext();
        }
    }
}
