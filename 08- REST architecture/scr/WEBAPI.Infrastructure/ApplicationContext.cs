using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.EntityConfigurations;
using WEBAPI.Infrastructure.SeedData;

namespace WEBAPI.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {

        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(new CategoryEntityTypeConfiguration().Configure);
            modelBuilder.Entity<Product>(new ProductEntityTypeConfiguration().Configure);

            modelBuilder.Seed();
        }
    }
}
